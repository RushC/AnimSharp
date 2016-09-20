using System;
using System.Threading;

namespace AnimSharp.Animate
{
    /// <summary>
    /// The generic verision of the IAnimation interface specifies a specific type
    /// for the value that is animated.
    /// </summary>
    /// 
    /// <typeparam name="T">
    /// the type of value that is animated by the IAnimation implementation.
    /// </typeparam>
    public abstract class Animation<T> : IAnimation
    {
        private object lockObject;      // The object to lock to prevent concurrent modification.

        /// <summary>
        /// Base constructor to be called by all subclasses.
        /// </summary>
        protected Animation()
        {
            // Initialize instance variables.
            this.lockObject = new object();
        }

        /// <summary>
        /// Advances the animation the specified amount of time.
        /// </summary>
        /// 
        /// <param name="milliseconds">
        /// the number of milliseconds to advance the animation.
        /// </param>
        public void Advance(long milliseconds)
        {
            // Prevent multiple threads from advancing the animation
            // at the same time.
            lock (this.lockObject)
            {
                // Ensure the number of milliseconds is non-negative.
                if (milliseconds < 0)
                    throw new InvalidOperationException(
                        "Number of milliseconds must be non-negative"
                    );

                // Ensure the animation hasn't ended yet.
                if (this.Ended)
                    throw new InvalidOperationException(
                        "Cannot advance an animation that has already ended."
                    );

                // Started Event
                if (!this.Started && this.AnimationStarted != null)
                    this.AnimationStarted(this, new AnimationEventArgs(this));

                // Pre Increment Event
                if (this.AnimationPreIncrement != null)
                    this.AnimationPreIncrement(this, new AnimationEventArgs(this));

                // Increment Logic
                this.ElapsedTime += milliseconds;
                if (this.ElapsedTime > this.Duration)
                    this.ElapsedTime = this.Duration;

                // Increment Event
                if (this.AnimationIncremented != null)
                    this.AnimationIncremented(this, new AnimationEventArgs(this));

                // Post Increment Event
                if (this.AnimationPostIncrement != null)
                    this.AnimationPostIncrement(this, new AnimationEventArgs(this));

                // Ended Event
                if (this.Ended && this.AnimationEnded != null)
                    this.AnimationEnded(this, new AnimationEventArgs(this));
            }
        }

        /// <summary>
        /// Called when the Animation has finished.
        /// </summary>
        public event AnimationEventHandler AnimationEnded;

        /// <summary>
        /// Called when the Animation is started.
        /// </summary>
        public event AnimationEventHandler AnimationStarted;

        /// <summary>
        /// Called when the Animation is stopped via the Stop
        /// method.
        /// </summary>
        public event AnimationEventHandler AnimationStopped;

        /// <summary>
        /// Called before the animation is incremented.
        /// </summary>
        public event AnimationEventHandler AnimationPreIncrement;

        /// <summary>
        /// Called every time the animation is incremented.
        /// </summary>
        public event AnimationEventHandler AnimationIncremented;

        /// <summary>
        /// Called after the animation is incremented.
        /// </summary>
        public event AnimationEventHandler AnimationPostIncrement;

        public void Await()
        {
            this.Await(0);
        }

        public void Await(int milliseconds)
        {
            if (milliseconds < 0)
                throw new ArgumentException("Cannot wait for fewer than 0 milliseconds");

            // Ensure the animation's state cannot be changed while it is
            // being checked.
            lock (this.lockObject)
            {
                while (!this.Ended)
                {
                    // Wait for the animation to alert this thread when it is
                    // finished.
                    this.AnimationEnded += (_, __) =>
                    {
                        lock (this.lockObject)
                            Monitor.PulseAll(this);
                    };

                    Monitor.Wait(this);
                }
            }

            // Wait for the specified amount of extra time.
            if (milliseconds > 0)
                Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// Gets the value at the current stage in the
        /// animation.
        /// </summary>
        public abstract T CurrentValue { get; }

        /// <summary>
        /// Gets the duration of the animation in milliseconds.
        /// </summary>
        public long Duration { get; protected set; }

        /// <summary>
        /// Gets the amount of time that has elapsed in the animation
        /// in milliseconds.
        /// </summary>
        public abstract long ElapsedTime { get; protected set; }

        /// <summary>
        /// Gets whether or not the animation has completed.
        /// </summary>
        public bool Ended
        {
            get
            {
                return this.ElapsedTime == this.Duration;
            }
        }

        /// <summary>
        /// Gets the ending value of the animation.
        /// </summary>
        public abstract T EndValue { get; }

        public void Finish()
        {
            // Stop the animation.
            this.Stop();

            // Perform one final increment.
            var remainingTime = this.Duration - this.ElapsedTime;
            if (remainingTime > 0)
                this.Advance(remainingTime);
        }

        /// <summary>
        /// Resets the animation to its initial state.
        /// </summary>
        public void Reset()
        {
            lock (this.lockObject)
                // Reset the elapsed time.
                this.ElapsedTime = 0;
        }

        /// <summary>
        /// Starts animating from the start value to the end value
        /// using the Animator class.
        /// </summary>
        public void Start()
        {
            // Ensure the Animation has not started yet.
            if (this.Started)
                throw new InvalidOperationException(
                    "Animation has already been started"
                );

            // If the animation lasts zero seconds, end it immediately.
            if (this.Duration == 0)
            {
                if (this.AnimationEnded != null)
                    this.AnimationEnded(this, new AnimationEventArgs(this));

                return;
            }

            // Use the animator to start the animation.
            Animator.Instance.Animate(this);
        }

        /// <summary>
        /// Gets whether or not the animation has started.
        /// </summary>
        public bool Started
        {
            get
            {
                return this.ElapsedTime > 0;
            }
        }

        /// <summary>
        /// Gets the starting value of the animation.
        /// </summary>
        public abstract T StartValue { get; }

        public void Stop()
        {
            // Remove the animation from the animator.
            Animator.Instance.StopAnimation(this);

            // Trigger AnimationStopped event.
            if (this.AnimationStopped != null)
                this.AnimationStopped(this, new AnimationEventArgs(this));
        }

        public void Then(PostAnimationAction action)
        {
            lock (this.lockObject)
            {
                // Animation already ended. Perform action immediately.
                if (this.Ended)
                    new Thread(() => action()).Start();

                else
                    // Perform action when animation ends.
                    this.AnimationEnded += (_, __) => new Thread(() => action()).Start();
            }
        }
    }
}

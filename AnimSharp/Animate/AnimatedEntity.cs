using AnimSharp.Interpolate;

using System;
using System.Collections.Generic;
using System.Threading;

namespace AnimSharp.Animate
{
    /// <summary>
    /// An AnimatedEntity represents an entity that can be animated. The exact type
    /// of value that is animated is dependent on the implementation.
    /// 
    /// The AnimatedEntity class provides a common base for any subclass that may
    /// need to be animated, including a default interpolator and default duration
    /// to be used for animations that don't specify them. The AnimatedEntity class
    /// also provides methods to keep track of what animations it is running at any
    /// given time.
    /// </summary
    /// 
    /// <typeparam name="T">
    /// the type of objects that can be animated in the AnimatedEntity.
    /// </typeparam>
    public abstract class AnimatedEntity
    {
        private List<IAnimation> animations;        // Used by Animations property.
        private ReaderWriterLock readerWriterLock;  // Object to lock to prevent concurrent modification.

        /// <summary>
        /// Sets the DefaultDuration and DefaultInterpolator to the default
        /// values.
        /// </summary>
        protected AnimatedEntity()
        {
            // Initialize instance variables.
            this.animations = new List<IAnimation>();
            this.readerWriterLock = new ReaderWriterLock();

            // Initial property values.
            this.DefaultDuration = 400;
            this.DefaultInterpolator = Interpolators.Default;
        }

        /// <summary>
        /// Adds the specified animation to the list of running animations.
        /// 
        /// When the animation ends, it will automatically be removed from the list.
        /// </summary>
        /// 
        /// <param name="animation">
        /// the animation to add to the running animation list. This animation be added
        /// before it is started.
        /// </param>
        /// 
        /// <returns>
        /// the added animation. The animation will not be started upon return.
        /// </returns>
        protected void AddAnimation(IAnimation animation)
        {
            // Remove the animation from the animation list when it ends.
            animation.AnimationEnded += RemoveAnimation;

            // Add the animation to the list of animations.
            this.readerWriterLock.AcquireWriterLock(Timeout.Infinite);
            this.animations.Add(animation);
            this.readerWriterLock.ReleaseWriterLock();

            this.OnAnimationAdded(animation);
        }

        /// <summary>
        /// The animations currently running on the animated
        /// entity.
        /// </summary>
        protected IAnimation[] Animations
        {
            get
            {
                if (!this.readerWriterLock.IsWriterLockHeld)
                    this.readerWriterLock.AcquireReaderLock(Timeout.Infinite);

                var animationsArray = this.animations.ToArray();

                if (this.readerWriterLock.IsReaderLockHeld)
                    this.readerWriterLock.ReleaseReaderLock();

                return animationsArray;
            }
        }

        /// <summary>
        /// Blocks the current thread until all animations
        /// are completed.
        /// 
        /// Only the animations added before this method is
        /// called will be waited for. This method may return
        /// before an animation finishes if the animation is
        /// added after this method is called.
        /// </summary>
        /// 
        /// <remarks>
        /// An animation is only considered completed when
        /// the Ended property becomes true. If an Animation
        /// is stopped or for some reason prevented from
        /// advancing, it is possible this method will never
        /// return. Use this method with caution.
        /// 
        /// One should also be wary about calling this method
        /// during events. If the thread handling the event
        /// created a control, that control will be unable
        /// to update itself, since the thread that created
        /// it is the only thread allowed to modify it and is
        /// blocked. This can be counterproductive if an
        /// animation is intended to animate the control,
        /// as it will simply result in the control freezing
        /// for the duration of the animation.
        /// </remarks>
        public void Await()
        {
            this.Await(0);
        }

        /// <summary>
        /// Blocks the current thread until all animations
        /// are completed and then sleeps for the specified
        /// amount of time.
        /// 
        /// Only the animations added before this method is
        /// called will be waited for. This method may return
        /// before an animation finishes if the animation is
        /// added after this method is called.
        /// </summary>
        /// 
        /// <param name="milliseconds">
        /// the amount of time in milliseconds to sleep for
        /// after the animations finish.
        /// </param>
        /// 
        /// <exception cref="System.ArgumentException">
        /// if the milliseconds value is less than zero.
        /// </exception>
        /// 
        /// <remarks>
        /// An animation is only considered completed when
        /// the Ended property becomes true. If an Animation
        /// is stopped or for some reason prevented from
        /// advancing, it is possible this method will never
        /// return. Use this method with caution.
        /// 
        /// One should also be wary about calling this method
        /// during events. If the thread handling the event
        /// created a control, that control will be unable
        /// to update itself, since the thread that created
        /// it is the only thread allowed to modify it and is
        /// blocked. This can be counterproductive if an
        /// animation is intended to animate the control,
        /// as it will simply result in the control freezing
        /// for the duration of the animation.
        /// </remarks>
        public void Await(int milliseconds)
        {
            if (milliseconds < 0)
                throw new ArgumentException("Cannot wait for fewer than 0 milliseconds");

            // Keep track of the currently running animations.
            this.readerWriterLock.AcquireReaderLock(Timeout.Infinite);
            var animations = new List<IAnimation>(this.Animations);
            this.readerWriterLock.ReleaseReaderLock();

            // Have each animation remove itself from the list of animations
            // and alert this thread when it's finished.
            foreach (var animation in this.Animations)
                animation.AnimationEnded += (a, __) =>
                {
                    lock (this)
                    {
                        animations.Remove(a);
                        Monitor.PulseAll(this);
                    }
                };

            // Wait until every animation alerts this thread.
            lock (this)
            {
                while (animations.Count > 0)
                    Monitor.Wait(this);
            }

            // Wait for the specified amount of extra time.
            if (milliseconds > 0)
                Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// The duration of animations when no duration is specified. The
        /// duration is in milliseconds.
        /// 
        /// This duration can be changed at any time, but it is 400 milliseconds
        /// to start.
        /// </summary>
        public long DefaultDuration { get; set; }

        /// <summary>
        /// The Interpolator to be used for animations when no interpolator
        /// is specified.
        /// 
        /// This interpolator can be changed at any time, but it is the
        /// Default interpolator to start.
        /// </summary>
        public Interpolator DefaultInterpolator { get; set; }

        /// <summary>
        /// Immediately completes every animation.
        /// </summary>
        public void Finish()
        {
            this.readerWriterLock.AcquireReaderLock(Timeout.Infinite);
            var animations = this.Animations;
            this.readerWriterLock.ReleaseReaderLock();

            // Immediately finish every animation.
            foreach (var animation in animations)
                animation.Finish();

            // Call OnAnimationsFinished method.
            this.OnAnimationsFinished();
        }

        /// <summary>
        /// Called when a new animation is added to the animation list.
        /// </summary>
        /// 
        /// <param name="addedAnimation">
        /// the new animation that was added to the animation list,
        /// </param>
        protected virtual void OnAnimationAdded(IAnimation addedAnimation) { }

        /// <summary>
        /// Called when the animations are finished via the Finish method.
        /// </summary>
        protected virtual void OnAnimationsFinished() { }

        /// <summary>
        /// Called when the animations are stopped via the Stop method.
        /// </summary>
        protected virtual void OnAnimationsStopped() { }

        /// <summary>
        /// Removes the specified animation from the list of running animations.
        /// </summary>
        /// 
        /// <param name="animation">
        /// the animation to remove from the list of running animations. If this
        /// animation was not returned from one of the overloaded AddAnimation
        /// methods, it will not be in the list.
        /// </param>
        protected void RemoveAnimation(IAnimation animation, AnimationEventArgs args)
        {
            this.readerWriterLock.AcquireWriterLock(Timeout.Infinite);

            // Check if the animation is successfully removed.
            if (this.animations.Remove(animation))
                // Remove the event handler for the animation.
                animation.AnimationEnded -= RemoveAnimation;

            this.readerWriterLock.ReleaseWriterLock();
        }

        /// <summary>
        /// Stops all animations currently running.
        /// </summary>
        public void Stop()
        {
            this.readerWriterLock.AcquireWriterLock(Timeout.Infinite);

            // Stop and remove each animation.
            for (int i = this.animations.Count - 1; i >= 0; i--)
            {
                var animation = this.animations[i];

                // Stop animation.
                this.animations[i].Stop();

                // Remove animation end listener.
                this.animations[i].AnimationEnded -= RemoveAnimation;

                // Remove animation.
                this.animations.RemoveAt(i);
            }

            // Call Animations Stopped event method.
            this.OnAnimationsStopped();

            this.readerWriterLock.ReleaseWriterLock();

            // If Await or Then were called, they are still waiting for
            // animations to end to wake them up. Wake them up manually.
            lock (this)
                Monitor.PulseAll(this);
        }

        /// <summary>
        /// Performs the specified action after all animations finish.
        /// </summary>
        /// 
        /// <param name="action">
        /// the action to perform after the animation finishes.
        /// </param>
        /// 
        /// <remarks>
        /// This is an alternative to the Await method. The difference
        /// between this method and Await is that this method will not
        /// block the current thread, and will instead perform the 
        /// specified action asynchronously when the animations finish.
        /// 
        /// If no animations are running when this method is called, 
        /// the action will be started immediately, though still
        /// asynchronously.
        /// </remarks>
        public void Then(PostAnimationAction action)
        {
            // Keep track of the running animations.
            this.readerWriterLock.AcquireReaderLock(Timeout.Infinite);
            var animations = new List<IAnimation>(this.Animations);
            this.readerWriterLock.ReleaseReaderLock();

            // Have each animation remove itself from the list and
            // alert the new thread when it finishes.
            foreach (var animation in this.Animations)
                animation.AnimationEnded += (a, __) =>
                {
                    lock (this)
                    {
                        animations.Remove(a);
                        Monitor.PulseAll(this);
                    }
                };

            // Start a new thread that will wait until the animations are
            // completed and then perform the action.
            new Thread(() =>
            {
                lock (this)
                {
                    while (animations.Count > 0)
                        Monitor.Wait(this);
                }

                action();
            }).Start();            
        }
    }
}

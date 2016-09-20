using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace AnimSharp.Animate
{
    /// <summary>
    /// An Animator is used to run Animation objects.
    /// 
    /// An Animator uses a special thread to increment Animations.
    /// Since many Animator instances would result in multiple
    /// desynchronized threads, only one Animator instance exists.
    /// It can be retrieved via the static Instance property.
    /// 
    /// In order to allow specific objects controlling multiple animations
    /// to know when their last animation has been incremented in a frame,
    /// animations will be guaranteed to be incremented in the order they
    /// were started. This means that whichever animation was started
    /// last can safely use its increment listener to make changes
    /// assuming that all animations have been incremented.
    /// </summary>
    public sealed class Animator
    {
        private static readonly Animator instance = new Animator();     // The single Animator instance shared by all classes.

        private List<IAnimation> animations;                            // The list of animations currently being animated.
        private object lockObject;                                      // The object to lock to prevent concurrent modifications.

        // Private to prevent outside classes from creating Animator instances.
        private Animator()
        {
            // Initialize instance variables.
            this.animations = new List<IAnimation>();
            this.lockObject = new object();

            // Initial property values.
            this.Framerate = 60;
        }
        
        /// <summary>
        /// Begins animating the specified Animation.
        /// </summary>
        /// 
        /// <param name="animation">
        /// the Animation to be started.
        /// </param>
        public void Animate(IAnimation animation)
        {
            // Prevent concurrent modifications to the animation
            // list.
            lock (this.lockObject)
            {
                // Add the animation to the list of animations.
                this.animations.Add(animation);

                // If the animator isn't running, start running it in
                // a new thread.
                if (!this.Running)
                    new Thread(Run).Start();
            }
        }

        /// <summary>
        /// Gets the duration of each animation frame in milliseconds.
        /// </summary>
        public int FrameDuration
        {
            get { return 1000 / Framerate; }
        }

        /// <summary>
        /// Gets or sets number of animation frames that should be processed 
        /// each second.
        /// 
        /// Changing this value will only impact the duration of any animations
        /// if their durations are not evenly divisible by the duration of a
        /// frame, which is directly affected by the framerate. 
        /// </summary>
        public int Framerate { get; set; }

        /// <summary>
        /// Gets the single Animator instance.
        /// </summary>
        public static Animator Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// Advances all animations the specified amount of time.
        /// </summary>
        /// 
        /// <param name="frameDuration">
        /// the duration of the animation frame to process in
        /// milliseconds.
        /// </param>
        private void ProcessAnimationFrame(int frameDuration)
        {
            // Iterate through each of the animations.
            for (var i = 0; i < animations.Count; i++)
            {
                // Advance each animation.
                if (!animations[i].Ended)
                    animations[i].Advance(frameDuration);

                // If the animation is finished, remove it from the
                // list of animations.
                if (animations[i].Ended)
                {
                    animations.RemoveAt(i);

                    // Since an animation was removed at the current
                    // index, the next animation to be iterated through
                    // is now at the current index. The index must be
                    // be decremented to offset the for loop's increment.
                    i--;
                }
            }
        }

        /// <summary>
        /// Runs all of the animations in the list.
        /// </summary>
        private void Run()
        {
            // Begin running.
            this.Running = true;

            // Use stopwatches to time how long processing each animation
            // frame takes and how long sleeping lasts.
            var processingStopwatch = new Stopwatch();
            var sleepingStopwatch = new Stopwatch();

            long oversleepDuration = 0;
            long processingDuration = 0;

            // Loop until the animator is stopped.
            while (this.Running)
            {
                // Sleep between animation frames.
                var frameDuration = this.FrameDuration;

                // Correct for processing time and oversleep.
                processingDuration = processingStopwatch.ElapsedMilliseconds;
                var sleepDuration = frameDuration - processingDuration - oversleepDuration;

                // If the sleep duration is less than zero, then the last animation frame took longer
                // than the frame duration, which means we need to start the next frame immediately.
                if (sleepDuration <= 0)
                {
                    // This also means the frame duration was longer than expected.
                    frameDuration -= (int)sleepDuration;
                    oversleepDuration = 0;
                }

                else
                {
                    // Start timing to see how long the thread sleeps.
                    sleepingStopwatch.Restart();

                    // Sleep
                    Thread.Sleep((int)sleepDuration);

                    // Determine how much the thread overslept.
                    sleepingStopwatch.Stop();
                    oversleepDuration = sleepingStopwatch.ElapsedMilliseconds - sleepDuration;
                }

                // Start timing to see how long processing the next animation frame takes.
                processingStopwatch.Restart();

                // Ensure the list of animations is not modified
                // while processing an animation frame.
                lock (this.lockObject)
                {
                    // Process an animation frame.
                    this.ProcessAnimationFrame(frameDuration + (int)oversleepDuration);

                    // If there are no more animations, wait for a notification
                    // that an animation has been added.
                    if (this.animations.Count == 0)
                    {
                        this.Running = false;
                        break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Gets whether or not the Animator is currently animating
        /// any values.
        /// </summary>
        public bool Running { get; private set; }

        /// <summary>
        /// Stops animating the specified animation.
        /// </summary>
        /// 
        /// <param name="animation">
        /// the animation to stop animating.
        /// </param>
        /// 
        /// <remarks>
        /// When an animation is stopped, all trace of it is removed
        /// from the Animator, with no plans made to continue it. If
        /// the animation needs to be continued, the Animate method
        /// will need to be called again, and the animation will
        /// continue where it left off.
        /// </remarks>
        public void StopAnimation(IAnimation animation)
        {
            // Wait until a frame is not being processed.
            lock (this.lockObject)
            {   
                // Remove animation.
                this.animations.Remove(animation);
            }
        }
    }
}

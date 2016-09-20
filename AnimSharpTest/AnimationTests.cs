using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using AnimSharp.Animate;

namespace AControllerTest
{
    [TestClass]
    public class AnimationTests
    {
        /// <summary>
        /// Tests if awaiting animations awaits for the appropriate amount of time.
        /// </summary>
        [TestMethod]
        public void AnimationAwaitTest()
        {
            AnimationAwaitTestCase(0, 0);
            AnimationAwaitTestCase(0, 200);
            AnimationAwaitTestCase(0, 500);
            AnimationAwaitTestCase(200, 0);
            AnimationAwaitTestCase(200, 200);
            AnimationAwaitTestCase(200, 500);
            AnimationAwaitTestCase(500, 0);
            AnimationAwaitTestCase(500, 200);
            AnimationAwaitTestCase(500, 500);
        }

        /// <summary>
        /// Tests if awaiting a specified amount of time for a single animation lasting the 
        /// specified amount of time waits for the appropriate amount of time.
        /// </summary>
        /// 
        /// <param name="animationDuration">
        /// the duration of the animation in milliseconds.
        /// </param>
        /// 
        /// <param name="awaitduration">
        /// the amount of time to await after the animation finishes.
        /// </param>
        public void AnimationAwaitTestCase(long animationDuration, int awaitduration)
        {
            // Setup the animation.
            var animation = new ValueAnimation(0, 0, animationDuration);

            // Time the waiting time.
            var stopwatch = new Stopwatch();

            // Start the animation.
            animation.Start();
            stopwatch.Start();

            // Await
            animation.Await(awaitduration);

            // Stop the stopwatch.
            stopwatch.Stop();

            // The error must be within the acceptable amount.
            var elapsed = stopwatch.ElapsedMilliseconds;
            var error = Math.Abs(elapsed - animationDuration) - awaitduration;
            var acceptableError = Animator.Instance.FrameDuration;

            Assert.IsTrue(
                error <= acceptableError,
                String.Format(
                    "Waited for {0} milliseconds instead of {1} milliseconds for an animation lasting {2} "
                    + "milliseconds and await time of {3} milliseconds with an allotted error of {4} milliseconds.",
                    elapsed,
                    animationDuration + awaitduration,
                    animationDuration,
                    awaitduration,
                    acceptableError
                )
            );
        }

        /// <summary>
        /// Tests if animations run for the amount of time they are set to
        /// run for, with a reasonable amount of error.
        /// </summary>
        [TestMethod]
        public void AnimationDurationTest()
        {
            this.AnimationDurationTestCase(60, 400);
            this.AnimationDurationTestCase(30, 400);
            this.AnimationDurationTestCase(10, 400);
            this.AnimationDurationTestCase(60, 0);
            this.AnimationDurationTestCase(30, 0);
            this.AnimationDurationTestCase(10, 0);
            this.AnimationDurationTestCase(60, 10000);
            this.AnimationDurationTestCase(30, 10000);
            this.AnimationDurationTestCase(10, 10000);
        }

        /// <summary>
        /// Tests if a single animation runs for the specified amount of time
        /// at the specified framerate.
        /// </summary>
        /// 
        /// <param name="framerate">
        /// the framerate that the test animation should run. This framerate
        /// is used to determine how much the animation can overshoot its
        /// time frame.
        /// </param>
        /// 
        /// <param name="duration">
        /// the amount of time in milliseconds the animation should run.
        /// </param>
        private void AnimationDurationTestCase(int framerate, long duration)
        {
            // Setup the animation.
            var animation = new ValueAnimation(0, 0, duration);
            Animator.Instance.Framerate = framerate;

            // Create a stopwatch to time the animation.
            var stopwatch = new Stopwatch();

            // Have the stopwatch stopped at the end of the animation.
            animation.AnimationEnded += (_, __) => stopwatch.Stop();

            // Start the stopwatch and animation.
            stopwatch.Start();
            animation.Start();

            // Wait for the animation to stop.
            while (stopwatch.IsRunning);

            // The error must be within the acceptable amount.
            var elapsed = stopwatch.ElapsedMilliseconds;
            var error = Math.Abs(elapsed - animation.Duration);
            var acceptableError = Animator.Instance.FrameDuration;

            Assert.IsTrue(
                error <= acceptableError,
                string.Format(
                    "Animation lasted {0} milliseconds instead of {1} milliseconds with a framerate of {2} FPS "
                    + " with an allotted error of {3} milliseconds.",
                    elapsed,
                    animation.Duration,
                    framerate,
                    acceptableError
                )
            );
        }
    }
}

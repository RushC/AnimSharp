using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimSharp.Animate
{
    /// <summary>
    /// AnimationEventArgs is a type of EventArgs that contains useful information
    /// pertinent to the state of an IAnimation implementation when an animation
    /// related event is called.
    /// </summary>
    public class AnimationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the duration of the triggering animation in milliseconds.
        /// </summary>
        public long Duration { get; private set; }

        /// <summary>
        /// Gets the amount of time in milliseconds that has elapsed in the
        /// triggering animation.
        /// </summary>
        public long ElapsedTime { get; private set; }

        /// <summary>
        /// Constructs a new set of AnimationEventArgs with the specified
        /// arguments.
        /// </summary>
        /// 
        /// <param name="animation">
        /// the animation to retrieve the values of each of the arguments
        /// from.
        /// </param>
        public AnimationEventArgs(IAnimation animation)
        {
            this.Duration = animation.Duration;
            this.ElapsedTime = animation.ElapsedTime;
        }
    }
}

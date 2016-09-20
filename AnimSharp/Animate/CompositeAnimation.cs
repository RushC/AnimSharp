using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimSharp.Animate
{
    /// <summary>
    /// A CompositeAnimation is a type of animation that represents multiple
    /// collective animations. A CompositeAnimation should be used to represent
    /// a complex animation that is created as the result of several simpler
    /// animations running together.
    /// </summary>
    public class CompositeAnimation<T> : Animation<T[]>
    {
        /// <summary>
        /// Constructs a new CompositeAnimation that is composed of the 
        /// specified animations.
        /// </summary>
        /// 
        /// <param name="animations">
        /// the animations that should make up the constructed composite
        /// animation.
        /// </param>
        public CompositeAnimation(params Animation<T>[] animations)
        {
            this.Animations = animations;

            // The duration of the longest running animation is the duration
            // of the composite animation.
            this.Duration = animations.Max(animation => animation.Duration);
        }

        /// <summary>
        /// Gets the Animation objects that compose this CompositeAnimation.
        /// </summary>
        public Animation<T>[] Animations { get; private set; }

        /// <summary>
        /// Gets an array of current values for each of the animations that compose
        /// the composite animation. The order of the current values matches the order
        /// of the animations in the Animations array.
        /// </summary>
        public override T[] CurrentValue
        {
            get
            {
                return this.Animations.ToList().ConvertAll(a => a.CurrentValue).ToArray();
            }
        }
        
        /// <summary>
        /// Gets or sets the anount of time that has elapsed in the animation in
        /// milliseconds.
        /// </summary>
        public override long ElapsedTime
        {
            get
            {
                // Some animations may end sooner than others, therefore the longest
                // animation has the best representation of the elapsed time.
                return this.Animations.Max(animation => animation.ElapsedTime);
            }

            protected set
            {
                // Advance every animation that hasn't ended yet.
                foreach (var animation in this.Animations)
                    if (!animation.Ended)
                    {
                        var elapsedTime = value - animation.ElapsedTime;
                        animation.Advance(elapsedTime);
                    }
            }
        }

        /// <summary>
        /// Gets an array of end values for each of the animations that compose
        /// the composite animation. The order of the end values matches the order
        /// of the animations in the Animations array.
        /// </summary>
        public override T[] EndValue
        {
            get
            {
                return this.Animations.ToList().ConvertAll(a => a.EndValue).ToArray();
            }
        }

        /// <summary>
        /// Gets an array of start values for each of the animations that compose
        /// the composite animation. The order of the start values matches the order
        /// of the animations in the Animations array.
        /// </summary>
        public override T[] StartValue
        {
            get
            {
                return this.Animations.ToList().ConvertAll(a => a.StartValue).ToArray();
            }
        }
    }
}

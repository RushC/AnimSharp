using System.Drawing;

namespace AnimSharp.Animate
{
    /// <summary>
    /// The handler used for all events declared in the AnimatedColor
    /// class.
    /// </summary>
    /// 
    /// <param name="source">
    /// the AnimatedColor instance that triggered this event.
    /// </param>
    public delegate void AnimatedColorHandler(AnimatedColor source);

    /// <summary>
    /// An AnimatedColor represents a color that can have its values changed.
    /// 
    /// AnimatedColor is basically a wrapper class for a color that supplies convenience
    /// methods to animates the color's values.
    /// 
    /// AnimatedColor is separate from an AnimatedValue and an AnimatedObject because of the unique
    /// way a color must be instantiated and because colors are immutable. For this reason, this class
    /// is necessary to supply special logic needed for animating colors.
    /// 
    /// An AnimatedColor can only run one animation at a time.
    /// </summary>
    public class AnimatedColor : AnimatedEntity
    {
        /// <summary>
        /// Constructs a new AnimatedColor instance that acts as a wrapper for the specified color.
        /// </summary>
        /// 
        /// <param name="color">
        /// the color that the constructed AnimatedColor class acts as a wrapper for.
        /// </param>
        public AnimatedColor(Color color)
        {
            this.Color = color;
        }

        /// <summary>
        /// Animates from the specified color back to the current color.
        /// </summary>
        /// 
        /// <param name="oldColor">
        /// the color to animate to the current color from.
        /// </param>
        public void AnimateFrom(Color oldColor)
        {
            this.AnimateColor(oldColor, this.Color);
        }

        /// <summary>
        /// Animates the current color value to the specified color value.
        /// </summary>
        /// 
        /// <param name="newColor">
        /// the color to animate to.
        /// </param>
        public void AnimateTo(Color newColor)
        {
            this.AnimateColor(this.Color, newColor);
        }

        /// <summary>
        /// Animates from the starting color value to the ending color value, updating
        /// the instance's color value along the way.
        /// </summary>
        /// 
        /// <param name="startColor">
        /// the color to start animating from.
        /// </param>
        /// 
        /// <param name="endColor">
        /// the color to animate to.
        /// </param>
        private void AnimateColor(Color startColor, Color endColor)
        {
            // Only one animation can run at a time.
            if (this.Animations.Length > 0)
                return;

            // Create Color Animation
            var colorAnimation = new ColorAnimation(startColor, endColor, 
                                                    this.DefaultDuration, 
                                                    this.DefaultInterpolator);
            colorAnimation.AnimationIncremented += (_, __) =>
            {
                // Set Color
                this.Color = colorAnimation.CurrentColor;

                // Color Changed Event
                if (this.ColorChanged != null)
                    this.ColorChanged(this);
            };
            this.AddAnimation(colorAnimation);

            // Start Animation
            colorAnimation.Start();
        }

        /// <summary>
        /// Gets the color wrapped by this instance.
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// Called whenever the color is changed during an animation.
        /// </summary>
        public event AnimatedColorHandler ColorChanged;
    }
}

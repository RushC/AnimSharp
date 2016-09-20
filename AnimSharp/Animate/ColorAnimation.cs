using AnimSharp.Interpolate;

using System.Drawing;

namespace AnimSharp.Animate
{
    /// <summary>
    /// A ColorAnimation is a type of CompositeAnimation that consists of
    /// animations for each ARGB value between two System.Drawing.Color
    /// values.
    /// 
    /// In order to animate a color, the ARGB values of the color need to be
    /// animated. After each value is animated, the color should be updated.
    /// There are two ways to modify the values:
    /// 
    ///      1) Animate a single integer value representing the entire color 
    ///         (ex. 0xFFFF0000 -> 0xFF00FF00 for red to green)
    /// 
    ///      2) Animate each value independently and set the color after each
    ///         has been incremented.
    /// 
    /// While both methods will effectively end up at the same result, the
    /// first will result in a slightly more awkward color transition
    /// (for example, red to green may have gray as an intermediate color,
    /// which might seem strange). The second method will result in a 
    /// transition that is smoother and more direct. For this reason, this
    /// class opts for the second method.
    /// </summary>
    public class ColorAnimation : CompositeAnimation<double>
    {
        /// <summary>
        /// Constructs a new ColorAnimation that animates between the specified colors using
        /// the default interpolator that will last 400 miliseconds.
        /// </summary>
        /// 
        /// <param name="startColor">
        /// the color to start the animation from.
        /// </param>
        /// 
        /// <param name="endColor">
        /// the color to end the animation at.
        /// </param>
        public ColorAnimation(Color startColor, Color endColor)
            : this(startColor, endColor, 400, Interpolators.Default) { }

        /// <summary>
        /// Constructs a new ColorAnimation that animates between the specified colors using
        /// the specified interpolator that will last 400 miliseconds.
        /// </summary>
        /// 
        /// <param name="startColor">
        /// the color to start the animation from.
        /// </param>
        /// 
        /// <param name="endColor">
        /// the color to end the animation at.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to use to generate intermediate color values
        /// during the animation.
        /// </param>
        public ColorAnimation(Color startColor, Color endColor, Interpolator interpolator)
            : this(startColor, endColor, 400, interpolator) { }

        /// <summary>
        /// Constructs a new ColorAnimation that animates between the specified colors using
        /// the default interpolator for the specified duration.
        /// </summary>
        /// 
        /// <param name="startColor">
        /// the color to start the animation from.
        /// </param>
        /// 
        /// <param name="endColor">
        /// the color to end the animation at.
        /// </param>
        /// 
        /// <param name="duration">
        /// the amount of time in milliseconds that the animation should last.
        /// </param>
        public ColorAnimation(Color startColor, Color endColor, long duration)
            : this(startColor, endColor, duration, Interpolators.Default) { }

        /// <summary>
        /// Constructs a new ColorAnimation that animates between the specified colors using
        /// the specified interpolator for the specified duration.
        /// </summary>
        /// 
        /// <param name="startColor">
        /// the color to start the animation from.
        /// </param>
        /// 
        /// <param name="endColor">
        /// the color to end the animation at.
        /// </param>
        /// 
        /// <param name="duration">
        /// the amount of time in milliseconds that the animation should last.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to use to generate intermediate color values
        /// during the animation.
        /// </param>
        public ColorAnimation(Color startColor, Color endColor, long duration, Interpolator interpolator)
            : base(
                  new ValueAnimation(startColor.A, endColor.A, duration, interpolator),
                  new ValueAnimation(startColor.R, endColor.R, duration, interpolator),
                  new ValueAnimation(startColor.G, endColor.G, duration, interpolator),
                  new ValueAnimation(startColor.B, endColor.B, duration, interpolator)
              ) { }

        /// <summary>
        /// Constructs a System.Drawing.Color value from the given collection of
        /// ARGB color values.
        /// </summary>
        /// <param name="compositeValue">
        /// an array of four doubles that should represent the following values in order:
        /// 
        /// 0 - Alpha
        /// 1 - Red
        /// 2 - Green
        /// 3 - Blue
        /// 
        /// This array is expected to be retrieved from one of the following base class
        /// properties:
        /// 
        /// CompositeAnimation#CurrentValue
        /// CompositeAnimation#EndValue
        /// CompositeAnimation#StartValue
        /// </param>
        private Color ColorFromCompositeValue(double[] compositeValue)
        {
            var alpha = (int)compositeValue[0];
            var red = (int)compositeValue[1];
            var green = (int)compositeValue[2];
            var blue = (int)compositeValue[3];

            return Color.FromArgb(alpha, red, green, blue);
        }

        /// <summary>
        /// Gets the color at the current stage of the animation.
        /// </summary>
        public Color CurrentColor
        {
            get
            {
                return this.ColorFromCompositeValue(this.CurrentValue);
            }
        }

        /// <summary>
        /// Gets the ending color of the animation.
        /// </summary>
        public Color EndColor
        {
            get
            {
                return this.ColorFromCompositeValue(this.EndValue);
            }
        }

        /// <summary>
        /// Gets the starting color of the animation.
        /// </summary>
        public Color StartColor
        {
            get
            {
                return this.ColorFromCompositeValue(this.StartValue);
            }
        }
    }
}

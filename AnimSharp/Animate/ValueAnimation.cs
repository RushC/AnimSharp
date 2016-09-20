using AnimSharp.Interpolate;

namespace AnimSharp.Animate
{
    /// <summary>
    /// A ValueAnimation is a type of Animation intended for primitive numerical values.
    /// </summary>
    public class ValueAnimation : Animation<double>
    {
        /// <summary>
        /// Constructs a new Animation instance with the specified start and
        /// end values using the default interpolator and duration.
        /// </summary>
        /// 
        /// <param name="startValue">
        /// the starting value of the animation.
        /// </param>
        /// <param name="endValue">
        /// the ending value of the animation.
        /// </param>
        public ValueAnimation(double startValue, double endValue)
                       : this(startValue, endValue, Interpolators.Linear)
        { }

        /// <summary>
        /// Constructs a new Animation instance with the specified start value,
        /// end value, and duration using the default interpolator.
        /// </summary>
        /// 
        /// <param name="startValue">
        /// the starting value of the animation.
        /// </param>
        /// <param name="endValue">
        /// the ending value of the animation.
        /// </param>
        /// <param name="duration">
        /// the duration of the animation in milliseconds.
        /// </param>
        public ValueAnimation(double startValue, double endValue, long duration)
                       : this(startValue, endValue, duration, Interpolators.Linear)
        { }

        /// <summary>
        /// Constructs a new Animation instance with the specified start value,
        /// end value, and interpolator using a duration of 400 milliseconds.
        /// </summary>
        /// 
        /// <param name="startValue">
        /// the starting value of the animation.
        /// </param>
        /// <param name="endValue">
        /// the ending value of the animation.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function to use to determine intermediate values.
        /// </param>
        public ValueAnimation(double startValue, double endValue, Interpolator interpolator)
                       : this(startValue, endValue, 400, interpolator)
        { }

        /// <summary>
        /// Constructs a new Animation instance with the specified start value,
        /// end value, interpolator, and duration.
        /// </summary>
        /// 
        /// <param name="startValue">
        /// the starting value of the animation.
        /// </param>
        /// <param name="endValue">
        /// the ending value of the animation.
        /// </param>
        /// <param name="duration">
        /// the duration of the animation in milliseconds.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function to use to determine intermediate values.
        /// </param>
        public ValueAnimation(double startValue, double endValue,
                              long duration, Interpolator interpolator)
        {
            this._startValue = startValue;
            this._endValue = endValue;
            this.Duration = duration;
            this.Interpolator = interpolator;
        }

        /// <summary>
        /// Gets the value at the current stage in the
        /// animation.
        /// </summary>
        public override double CurrentValue
        {
            get
            {
                // Ensure the animation's value is not modified while
                // retrieving the current value.
                return Interpolation.Interpolate(
                    0, StartValue,
                    Duration, EndValue,
                    ElapsedTime, Interpolator
                );
            }
        }

        /// <summary>
        /// Gets or sets the amount of time that has elapsed in the animation in milliseconds.
        /// </summary>
        public override long ElapsedTime { get; protected set; }

        private double _endValue;
        /// <summary>
        /// Gets the ending value of the animation.
        /// </summary>
        public override double EndValue
        {
            get
            {
                return this._endValue;
            }
        }

        /// <summary>
        /// Gets the interpolation function used to determine the 
        /// current value at every stage in the animation.
        /// </summary>
        public Interpolator Interpolator { get; private set; }


        private double _startValue;
        /// <summary>
        /// Gets the starting value of the animation.
        /// </summary>
        public override double StartValue
        {
            get
            {
                return this._startValue;
            }
        }
    }
}

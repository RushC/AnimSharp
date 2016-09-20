using AnimSharp.Interpolate;

namespace AnimSharp.Animate
{
    /// <summary>
    /// The handler used for all events declared in the AnimatedValue
    /// class.
    /// </summary>
    /// 
    /// <param name="source">
    /// the AnimatedValue instance that triggered the event.
    /// </param>
    public delegate void AnimatedValueHandler(AnimatedValue source);

    /// <summary>
    /// An AnimatedValue represents a value that can be animated
    /// towards or away from. AnimatedValue is essentially a wrapper
    /// class for a primitive value that supplies convenience methods
    /// to start animations using the value as an animation.
    /// 
    /// An AnimatedValue can only have one animation running at a time.
    /// </summary>
    public class AnimatedValue : AnimatedEntity
    {
        private object lockObject;      // The object to lock to prevent concurrent modifications.

        /// <summary>
        /// Contructs a new AnimatedValue instance representing the
        /// specified value.
        /// </summary>
        /// 
        /// <param name="value">
        /// the value the constructed AnimatedValue instance should
        /// represent.
        /// </param>
        public AnimatedValue(double value)
        {
            this.Value = value;
            this.DefaultInterpolator = Interpolators.Default;
        }

        /// <summary>
        /// Animates from the specified value to the current value
        /// using the interpolator as the interpolation function.
        /// </summary>
        /// 
        /// <param name="value">
        /// the value to start the animation from.
        /// </param>
        public void AnimateFrom(double value)
        {
            // Create a new animation animating from the specified value
            // to the current value using the specified interpolator.
            var animation = new ValueAnimation(value, this._value, DefaultDuration, DefaultInterpolator);
            this.AddAnimation(animation);
            this.StartAnimation(animation);
        }

        /// <summary>
        /// Animates the current value to the specified new value using
        /// the specified interpolator as the interpolation function.
        /// </summary>
        /// 
        /// <param name="value">
        /// the value to animate the value to.
        /// </param>
        public void AnimateTo(double value)
        {
            // Create an animation animating from the current value to
            // the target value using the specified interpolator.
            var animation = new ValueAnimation(this._value, value);
            this.AddAnimation(animation);
            this.StartAnimation(animation);
        }

        /// <summary>
        /// Starts the specified animation.
        /// </summary>
        /// 
        /// <param name="animation">
        /// the animation to be started.
        /// </param>
        private void StartAnimation(ValueAnimation animation)
        {
            // Ensure the value is not already being animated.
            if (Animations.Length > 1)
            {
                // Remove the animation, since only one animation can run at a time.
                RemoveAnimation(animation, new AnimationEventArgs(animation));
                return;
            }

            // Update the value at every animation frame.
            animation.AnimationIncremented += (_, __) => Value = animation.CurrentValue;

            // Start the animation.
            animation.Start();
        }

        private double _value;
        /// <summary>
        /// Gets the value represented by this AnimatedValue instance.
        /// </summary>
        public double Value
        {
            get { return _value; }

            private set
            {
                // Prevent concurrent modification of the value.
                lock (this.lockObject)
                {
                    this._value = value;

                    // Call the value changed event.
                    if (ValueChanged != null)
                        ValueChanged(this);
                }
            }
        }

        /// <summary>
        /// Called when the Value property is changed.
        /// </summary>
        public event AnimatedValueHandler ValueChanged;
    }
}

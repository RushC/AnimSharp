using System.Drawing;

using AnimSharp.Interpolate;

namespace AnimSharp
{
    /// <summary>
    /// IAControl defines the interface for a control that can
    /// have several of its properties animated.
    /// </summary>
    public interface IAControl
    {
        /// <summary>
        /// Animates a primitive type property to its current value
        /// from the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to be animated. The name must be
        /// of a property that has a primitive type.
        /// </param>
        /// 
        /// <param name="value">
        /// the value to animate the property from.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolator to use for the animation. If no interpolator
        /// is specified, the default interpolator will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration of the animation in milliseconds. If no duration
        /// is specified, the default duration will be used.
        /// </param>
        /// 
        /// <exception cref="ArgumentException">
        /// if the property doesn't exist or does not have a primitive type.
        /// </exception>
        void AnimatePropertyFrom(string propertyName, double value,
                                 Interpolator interpolator = default(Interpolator),
                                 long duration = -1);

        /// <summary>
        /// Animates a color type property to its current value
        /// from the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to be animated. The name must be
        /// of a property that is of the type System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="value">
        /// the value to animate the property from.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolator to use for the animation. If no interpolator
        /// is specified, the default interpolator will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration of the animation in milliseconds. If no duration
        /// is specified, the default duration will be used.
        /// </param>
        /// 
        /// <exception cref="ArgumentException">
        /// if the property doesn't exist or does not have the type
        /// System.Drawing.Color.
        /// </exception>
        void AnimatePropertyFrom(string propertyName, Color value,
                                 Interpolator interpolator = default(Interpolator),
                                 long duration = -1);

        /// <summary>
        /// Animates a primitive type property from its current value
        /// to the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to be animated. The name must be
        /// of a property that has a primitive type.
        /// </param>
        /// 
        /// <param name="value">
        /// the value to animate the property to.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolator to use for the animation. If no interpolator
        /// is specified, the default interpolator will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration of the animation in milliseconds. If no duration
        /// is specified, the default duration will be used.
        /// </param>
        /// 
        /// <exception cref="ArgumentException">
        /// if the property doesn't exist or does not have a primitive type.
        /// </exception>
        void AnimatePropertyTo(string propertyName, double value, 
                               Interpolator interpolator = default(Interpolator),
                               long duration = -1);

        /// <summary>
        /// Animates a color type property from its current value
        /// to the specified value.
        /// </summary>
        /// 
        /// <param name="propertyName">
        /// the name of the property to be animated. The name must be
        /// of a property that is of the type System.Drawing.Color.
        /// </param>
        /// 
        /// <param name="value">
        /// the value to animate the property to.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolator to use for the animation. If no interpolator
        /// is specified, the default interpolator will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration of the animation in milliseconds. If no duration
        /// is specified, the default duration will be used.
        /// </param>
        /// 
        /// <exception cref="ArgumentException">
        /// if the property doesn't exist or does not have the type
        /// System.Drawing.Color.
        /// </exception>
        void AnimatePropertyTo(string propertyName, Color value,
                               Interpolator interpolator = default(Interpolator),
                               long duration = -1);

        /// <summary>
        /// Gets or sets the default duration for this IAControl's animations
        /// in milliseconds.
        /// </summary>
        long DefaultDuration { get; set; }

        /// <summary>
        /// Gets or sets the default interpolation function to be used for
        /// calculating intermediate values for this IAControl's animations.
        /// </summary>
        Interpolator DefaultInterpolator { get; set; }

        /// <summary>
        /// Animates the IAControl's size by stretching/condensing it by
        /// the specifed amount.
        /// </summary>
        /// 
        /// <param name="deltaWidth">
        /// the amount the width should be stretched/condensed. A positive
        /// value will stretch the width while a negative value will
        /// condense it.
        /// </param>
        /// 
        /// <param name="deltaHeight">
        /// the amount the height should be stretched/condensed. A positive
        /// value will sretch the height while a negative value will
        /// condense it.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void Scale(int deltaWidth, int deltaHeight,
                   Interpolator interpolator = default(Interpolator),
                   long duration = -1);

        /// <summary>
        /// Animates the IAControl's size by stretching/condensing it by
        /// the specifed amount.
        /// </summary>
        /// 
        /// <param name="widthFactor">
        /// the ratio of the width's current value to scale to, with 1.0
        /// being the width's current value.
        /// </param>
        /// 
        /// <param name="heightFactor">
        /// the ratio of the height's current value to scale to, with 1.0
        /// being the height's current value.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void Scale(float widthFactor, float heightFactor,
                   Interpolator interpolator = default(Interpolator),
                   long duration = -1);

        /// <summary>
        /// Animates the IAControl's size by stretching/condensing it by
        /// the specifed amount without moving its center point.
        /// </summary>
        /// 
        /// <param name="deltaWidth">
        /// the amount the width should be stretched/condensed. A positive
        /// value will stretch the width while a negative value will
        /// condense it.
        /// </param>
        /// 
        /// <param name="deltaHeight">
        /// the amount the height should be stretched/condensed. A positive
        /// value will sretch the height while a negative value will
        /// condense it.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void ScaleCentered(int deltaWidth, int deltaHeight,
                           Interpolator interpolator = default(Interpolator),
                           long duration = -1);

        /// <summary>
        /// Animates the IAControl's size by stretching/condensing it by
        /// the specifed amount without moving its center point.
        /// </summary>
        /// 
        /// <param name="widthFactor">
        /// the ratio of the width's current value to scale to, with 1.0
        /// being the width's current value.
        /// </param>
        /// 
        /// <param name="heightFactor">
        /// the ratio of the height's current value to scale to, with 1.0
        /// being the height's current value.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void ScaleCentered(float widthFactor, float heightFactor,
                           Interpolator interpolator = default(Interpolator),
                           long duration = -1);

        /// <summary>
        /// Animates the IAControl's size from the specified size
        /// to its current size.
        /// </summary>
        /// 
        /// <param name="width">
        /// the value the width should be animated from.
        /// </param>
        /// 
        /// <param name="height">
        /// the value the height should be animated from.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void ScaleFrom(int width, int height,
                       Interpolator interpolator = default(Interpolator),
                       long duration = -1);

        /// <summary>
        /// Animates the IAControl's size to the specified size
        /// from its current size.
        /// </summary>
        /// 
        /// <param name="width">
        /// the value the width should be animated to.
        /// </param>
        /// 
        /// <param name="height">
        /// the value the height should be animated to.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void ScaleTo(int width, int height,
                     Interpolator interpolator = default(Interpolator),
                     long duration = -1);

        /// <summary>
        /// Animates the IAControl's position the specified distance.
        /// </summary>
        /// 
        /// <param name="deltaX">
        /// the distance the IAControl should animate its
        /// position along the X-Axis. A negative value will move the
        /// IAControl left and a positive value will move it right.
        /// </param>
        /// 
        /// <param name="deltaY">
        /// the distance the IAControl should animate its
        /// position along the Y-Axis. A negative value will move the
        /// IAControl up and a positive value will move it down.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void Translate(int deltaX, int deltaY,
                       Interpolator interpolator = default(Interpolator),
                       long duration = -1);

        /// <summary>
        /// Animates the IAControl's position from the specified position
        /// to its current position.
        /// </summary>
        /// 
        /// <param name="x">
        /// the X-Position to translate the IAControl's position from.
        /// </param>
        /// 
        /// <param name="y">
        /// he Y-Position to translate the IAControl's position from.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void TranslateFrom(int x, int y,
                           Interpolator interpolator = default(Interpolator),
                           long duration = -1);

        /// <summary>
        /// Animates the IAControl's position to the specified position
        /// from its current position.
        /// </summary>
        /// 
        /// <param name="x">
        /// the X-Position to translate the IAControl's position to.
        /// </param>
        /// 
        /// <param name="y">
        /// he Y-Position to translate the IAControl's position to.
        /// </param>
        /// 
        /// <param name="interpolator">
        /// the interpolation function to be used for the animation. If
        /// an interpolation function is not specified, the default one
        /// will be used.
        /// </param>
        /// 
        /// <param name="duration">
        /// the duration in milliseconds that the animation should last.
        /// If no duration is specified, the default duration will be used.
        /// </param>
        void TranslateTo(int x, int y,
                         Interpolator interpolator = default(Interpolator),
                         long duration = -1);
    }
}

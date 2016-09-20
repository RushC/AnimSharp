using System;

namespace AnimSharp.Interpolate
{
    /// <summary>
    /// Calculates how much the output has elapsed based on
    /// how much the input has elapsed. The means of this
    /// calculation depends on the interpolation function
    /// used.
    /// </summary>
    /// 
    /// <param name="elapsed">
    /// a floating point number ranging roughly from 0.0 to 1.0
    /// representing the amount the input has elapsed from its 
    /// starting value to its ending value.
    /// </param>
    /// 
    /// <returns>
    /// a floating point number ranging roughly from 0.0 to 1.0
    /// representing the amount the output has elapsed from its
    /// starting value to its ending value.
    /// </returns>
    /// 
    /// <remarks>
    /// Both the parameter and the return value represent the
    /// amount the input and output have elapsed respectively.
    /// For both values, 0 means that the value is at its
    /// starting value while 1 means that the value is at its
    /// ending value. Every fraction in between represents an
    /// intermediate value between the start and end. For
    /// example, 0.5 represents a value exactly halfway
    /// between the starting and ending value. Any number
    /// above or below 1 or 0 represent values that have
    /// overshot or undershot the ending or starting values
    /// respectively.
    /// </remarks>
    public delegate double Interpolator(double elapsed);

    /// <summary>
    /// Interpolators is a utility class containing a variety of
    /// interpolation functions to be used for calculating
    /// interpolated values.
    /// 
    /// Each interpolator function can be called if desired, but
    /// their main utility is for use in the Interpolate methods
    /// defined in the Interpolation utility class.
    /// </summary>
    public static class Interpolators
    {
        /// <summary>
        /// Interpolates between values using a parabolic shape.
        /// 
        /// In terms of animation, this will create animations
        /// that start slow and speed up as they reach the end.
        /// </summary>
        public static readonly Interpolator Accelerating = x => Math.Pow(x, 2);

        /// <summary>
        /// Interpolates between values using an upside down parabolic 
        /// shape.
        /// 
        /// In terms of animation, this will create animations
        /// that start fast and slow down as hey reach the end.
        /// </summary>
        public static readonly Interpolator Decelerating = new InverseInterpolator(Accelerating).Interpolator;

        /// <summary>
        /// Interpolates between values in a straight line.
        /// 
        /// In terms of animation, this will create animations
        /// that move at a constant speed.
        /// </summary>
        public static readonly Interpolator Linear = x => x;

        /// <summary>
        /// The default interpolation function to be used when no specific
        /// interpolator is specified.
        /// 
        /// The default can be changed at any point in the program, but the
        /// starting default is Linear.
        /// </summary>
        public static Interpolator Default = Linear;
    }
}

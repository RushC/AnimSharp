namespace AnimSharp.Interpolate
{
    /// <summary>
    /// Interpolation is a utility class used for interpolating values.
    /// 
    /// Interpolation is useful for calculating an infinite number of
    /// intermediate values in between two established values. The
    /// values of these intermediate points are determined by the
    /// interpolation function that is used.
    /// 
    /// Because animation often involved stopping at multiple intervals
    /// in between states to emphasize the change of state, interpolation
    /// is used. The different types of interpolation functions can be
    /// used to adjust the animation's movement in regards to acceleration,
    /// deceleration, easing, and potentially even repetition.
    /// 
    /// To make use of various existing interpolation functions, utilize
    /// the Interpolators utility class.
    /// </summary>
    public static class Interpolation
    {
        /// <summary>
        /// Calculates an output for an input value given the
        /// two surrounding sets of input/output values using
        /// the specified interpolation function.
        /// </summary>
        /// 
        /// <param name="startInput">
        /// the input of the starting set of input/output values.
        /// </param>
        /// <param name="startOutput">
        /// the output of the starting set of input/output values.
        /// </param>
        /// <param name="endInput">
        /// the input of the ending set of input/output values.
        /// </param>
        /// <param name="endOutput">
        /// the output of the ending set of input/output values.
        /// </param>
        /// <param name="intermediateInput">
        /// the intermediate input value to find the intermedaite
        /// output value for.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function to use to calculate the
        /// intermediate output value.
        /// </param>
        /// 
        /// <returns>
        /// the interpolated value.
        /// </returns>
        public static double Interpolate(double startInput, double startOutput,
                                         double endInput, double endOutput,
                                         double intermediateInput, Interpolator interpolator)
        {
            // Determine how much the input has elapsed.
            var inputRange = endInput - startInput;
            var elapsedInput = intermediateInput - startInput;
            var elapsedInputFraction = elapsedInput / inputRange;

            // Interpolate the value based on the elapsed input.
            return Interpolate(startOutput, endOutput,
                elapsedInputFraction, interpolator);
        }

        /// <summary>
        /// Calculates an interpolated value between the start and
        /// the end values based on the elapsed input using the
        /// given interpolating function.
        /// </summary>
        /// 
        /// <param name="start">
        /// the starting value of the output.
        /// </param>
        /// <param name="end">
        /// the ending value of the output.
        /// </param>
        /// <param name="elapsed">
        /// a value roughly between 0.0 and 1.0 representing the
        /// amount the input as elapsed.
        /// </param>
        /// <param name="interpolator">
        /// the interpolation function to use to calculate the
        /// intermediate value.
        /// </param>
        /// 
        /// <returns>
        /// the interpolated value.
        /// </returns>
        public static double Interpolate(double start, double end, double elapsed,
                                         Interpolator interpolator)
        {
            // Calculate the elapsed output fraction from the elapsed input
            // using the interpolator.
            var elapsedOutputFraction = interpolator(elapsed);

            // Calculate the elapsed output based on the range between the start
            // and end values and the elapsed output fraction.
            var range = end - start;
            var elapsedOutput = range * elapsedOutputFraction;

            // Determine the final value by adding the elapsed output to the
            // start value.
            return start + elapsedOutput;
        }
    }
}


using System.Collections.Generic;

namespace AnimSharp.Interpolate
{
    /// <summary>
    /// A CompositeInterpolator can utilize existing interpolator
    /// functions to create a new modified interpolation function.
    /// 
    /// CompositeInterpolators may utilize any number of base
    /// interpolation functions to define new interpolation
    /// behavior. Behaviors of CompositeInterpolators can
    /// involve modifying the behavior of an interpolator,
    /// stringing together multiple interpolation functions,
    /// and many other behaviors. 
    /// 
    /// Most complex interpolation functions should generally
    /// utilize this class to combine the functionality of
    /// the base interpolators in the Interpolators utility
    /// class.
    /// </summary>
    public abstract class CompositeInterpolator
    {
        /// <summary>
        /// Gets the list of base interpolation functions.
        /// </summary>
        protected List<Interpolator> BaseInterpolators { get; private set; }

        /// <summary>
        /// Base constructor for CompositeInterpolators.
        /// </summary>
        public CompositeInterpolator()
        {
            BaseInterpolators = new List<Interpolator>();
        }

        /// <summary>
        /// Interpolates to a factor of the output given the factor of
        /// an input.
        /// 
        /// This method is returned as the interpolation function and
        /// should be overriden to supply the interpolation logic
        /// using the base interpolators.
        /// </summary>
        /// 
        /// <param name="x">
        /// the factor of the input.
        /// </param>
        /// 
        /// <returns>
        /// the factor of the output based on the factor of the input.
        /// </returns>
        /// 
        /// <see cref="Interpolator"/>
        protected abstract double Interpolate(double x);

        /// <summary>
        /// The interpolation function created from the base
        /// interpolation functions.
        /// </summary>
        public Interpolator Interpolator
        {
            get
            {
                return this.Interpolate;
            }
        }
    }
}

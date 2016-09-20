using System.Linq;

namespace AnimSharp.Interpolate
{
    /// <summary>
    /// An InverseInterpolator is a CompositeInterpolator that takes
    /// a single interpolation function and acts as its inverse.
    /// </summary>
    /// 
    /// <remarks>
    /// The inverse of a function is the function flipped over the
    /// line y = x.
    /// </remarks>
    public class InverseInterpolator : CompositeInterpolator
    {
        /// <summary>
        /// Constructs a new InverseInterpolator that acts as the inverse
        /// of the specified interpolation function.
        /// </summary>
        /// 
        /// <param name="baseInterpolator">
        /// the interpolation function to create the inverse of.
        /// </param>
        public InverseInterpolator(Interpolator baseInterpolator)
        {
            this.BaseInterpolators.Add(baseInterpolator);
        }

        protected override double Interpolate(double x)
        {
            // Only one base interpolation function is used.
            var baseInterpolator = this.BaseInterpolators.First();

            return 1 - baseInterpolator(1 - x);
        }
    }
}

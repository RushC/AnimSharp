using AnimSharp.Animate;
using AnimSharp.Interpolate;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AnimSharp
{
    public class APanel : Panel, IAControl
    {
        private AnimatedControl animationWrapper;       // Wrapper for this class to delegate animation
                                                        // functions to.

        /// <summary>
        /// Constructs a new APanel instance.
        /// </summary>
        public APanel()
        {
            this.animationWrapper = new AnimatedControl(this);
        }

        public void AnimatePropertyFrom(string propertyName, double value,
                                        Interpolator interpolator = default(Interpolator),
                                        long duration = -1)
        {
            // Resolve default values.
            interpolator = interpolator ?? this.DefaultInterpolator;
            if (duration < 0)
                duration = this.DefaultDuration;

            this.animationWrapper.AnimatePropertyFrom(propertyName, value, duration, interpolator);
        }

        public void AnimatePropertyFrom(string propertyName, Color value,
                                        Interpolator interpolator = default(Interpolator),
                                        long duration = -1)
        {
            // Resolve default values.
            interpolator = interpolator ?? this.DefaultInterpolator;
            if (duration < 0)
                duration = this.DefaultDuration;

            this.animationWrapper.AnimatePropertyFrom(propertyName, value, duration, interpolator);
        }

        public void AnimatePropertyTo(string propertyName, double value,
                                      Interpolator interpolator = default(Interpolator),
                                      long duration = -1)
        {
            // Resolve default values.
            interpolator = interpolator ?? this.DefaultInterpolator;
            if (duration < 0)
                duration = this.DefaultDuration;

            this.animationWrapper.AnimatePropertyTo(propertyName, value, duration, interpolator);
        }

        public void AnimatePropertyTo(string propertyName, Color value,
                                      Interpolator interpolator = default(Interpolator),
                                      long duration = -1)
        {
            // Resolve default values.
            interpolator = interpolator ?? this.DefaultInterpolator;
            if (duration < 0)
                duration = this.DefaultDuration;

            this.animationWrapper.AnimatePropertyTo(propertyName, value, duration, interpolator);
        }

        [Browsable(false)]
        public long DefaultDuration
        {
            get { return this.animationWrapper.DefaultDuration; }
            set { this.animationWrapper.DefaultDuration = value; }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Interpolator DefaultInterpolator
        {
            get { return this.animationWrapper.DefaultInterpolator; }
            set { this.animationWrapper.DefaultInterpolator = value; }
        }

        public void Scale(int deltaWidth, int deltaHeight,
                          Interpolator interpolator = default(Interpolator),
                          long duration = -1)
        {
            // Determine values to animate to.
            var width = this.Width + deltaWidth;
            var height = this.Height + deltaHeight;

            // Scale to values.
            this.ScaleTo(width, height, interpolator, duration);
        }

        public void Scale(float widthFactor, float heightFactor,
                          Interpolator interpolator = default(Interpolator),
                          long duration = -1)
        {
            // Determine values to animate to.
            var width = this.Width * widthFactor;
            var height = this.Height * heightFactor;

            // Scale to values.
            this.ScaleTo((int)width, (int)height, interpolator, duration);
        }

        public void ScaleCentered(int deltaWidth, int deltaHeight,
                                  Interpolator interpolator = default(Interpolator),
                                  long duration = -1)
        {
            // Determine the position that will keep the center point in place.
            var x = this.Left - (deltaWidth / 2) + (deltaWidth % 2);
            var y = this.Top - (deltaHeight / 2) + (deltaHeight % 2);

            // Scale/translate to values.
            this.Scale(deltaWidth, deltaHeight, interpolator, duration);
            this.TranslateTo(x, y, interpolator, duration);
        }

        public void ScaleCentered(float widthFactor, float heightFactor,
                          Interpolator interpolator = default(Interpolator),
                          long duration = -1)
        {
            // Determine the change in width/height.
            var deltaWidth = (this.Width * widthFactor) - this.Width;
            var deltaHeight = (this.Height * heightFactor) - this.Height;

            // Scale to values.
            this.ScaleCentered((int)deltaWidth, (int)deltaHeight, interpolator, duration);
        }

        public void ScaleFrom(int width, int height,
                              Interpolator interpolator = default(Interpolator),
                              long duration = -1)
        {
            this.AnimatePropertyFrom("Width", width, interpolator, duration);
            this.AnimatePropertyFrom("Height", height, interpolator, duration);
        }

        public void ScaleTo(int width, int height,
                            Interpolator interpolator = default(Interpolator),
                            long duration = -1)
        {
            this.AnimatePropertyTo("Width", width, interpolator, duration);
            this.AnimatePropertyTo("Height", height, interpolator, duration);
        }

        public void Translate(int deltaX, int deltaY,
                              Interpolator interpolator = default(Interpolator),
                              long duration = -1)
        {
            // Determine values to animate to.
            var x = this.Left + deltaX;
            var y = this.Top + deltaY;

            // Translate to values.
            this.TranslateTo(x, y, interpolator, duration);
        }

        public void TranslateFrom(int x, int y,
                                  Interpolator interpolator = default(Interpolator),
                                  long duration = -1)
        {
            this.AnimatePropertyFrom("Left", x, interpolator, duration);
            this.AnimatePropertyFrom("Top", y, interpolator, duration);
        }

        public void TranslateTo(int x, int y,
                                Interpolator interpolator = default(Interpolator),
                                long duration = -1)
        {
            this.AnimatePropertyTo("Left", x, interpolator, duration);
            this.AnimatePropertyTo("Top", y, interpolator, duration);
        }
    }
}

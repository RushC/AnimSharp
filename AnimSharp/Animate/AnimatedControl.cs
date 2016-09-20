using AnimSharp.Utility;

using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AnimSharp.Animate
{
    /// <summary>
    /// An AnimatedControl represents a control that can have its properties
    /// animated. AnimatedControl is essentially a wrapper class for a control
    /// that supplies convenience methods for animating the control's properties.
    /// 
    /// An AnimatedControl should always be used for controls instead of
    /// an AnimatedObject. An AnimatedObject is insufficient for animating a
    /// control due to the constraint of controls needing to be modified on the
    /// thread that created them. This makes the animation methods in AnimatedObject
    /// unsafe for controls.
    /// </summary>
    public class AnimatedControl : AnimatedObject
    {
        /// <summary>
        /// Constructs a new AnimatedControl instance that wraps the specified
        /// control.
        /// </summary>
        /// 
        /// <param name="control">
        /// the control to be wrapped by the constructed AnimatedControl instance.
        /// </param>
        public AnimatedControl(Control control) : base(control)
        {
            // Listen for when the control is disposed.
            control.Disposed += this.Control_Disposed;
        }

        /// <summary>
        /// Gets the Control that is animated by this AnimatedControl instance.
        /// </summary>
        public Control Control
        {
            get { return this.Object as Control; }
        }

        /// <summary>
        /// Called when the control is disposed.
        /// </summary>
        private void Control_Disposed(object sender, EventArgs e)
        {
            // Stop all animations.
            this.Stop();
        }

        protected override object GetPropertyValue(PropertyInfo property)
        {
            // If we are not on the thread that created the control, we may not be
            // retrieving the most up to date value of the property. If an invoke
            // is required, we need to grab the value after the control's creating
            // thread has finished whatever it is doing.
            if (this.Control.InvokeRequired && this.Control.IsHandleCreated)
            {
                // We must block the current thread until we can retrieve the property's value
                // from the control's creating thread.
                object value = null;
                this.Control.Invoke(new Action(() =>
                {
                    value = base.GetPropertyValue(property);
                }));

                return value;
            }
            else
                return base.GetPropertyValue(property);
        }

        /// <summary>
        /// Allows the wrapped control to redraw after the SuspendDrawing
        /// method was called on it.
        /// </summary>
        public void ResumeDrawing()
        {
            this.Control.ResumeDrawing();
        }

        protected override void SetPropertyValue(PropertyInfo property, object value)
        {
            // Controls can only be modified on the thread that created them. Since this
            // method is almost guaranteed to be called on a different thread, we need
            // to invoke the set on the UI thread.
            //
            // NOTE: There is no check for this.Control.Invoke for a reason. This method
            //       can potentially be called in the UI thread if the Finish method is
            //       called in response to some UI event. In such a case, it is possible
            //       for property sets to be applied out of order. This could be alleviated
            //       by using Invoke instead of BeginInvoke, but Invoke can potentially
            //       result in a deadlock. Instead, BeginInvoke is used in every case to
            //       ensure that all set operations are completed in order. The only case
            //       when BeginInvoke is not used is when a handle has not yet been created
            //       for the control.
            if (this.Control.IsHandleCreated)
                this.Control.BeginInvoke(new Action(() =>
                {
                    base.SetPropertyValue(property, value);
                }));
            else
                base.SetPropertyValue(property, value);
        }

        /// <summary>
        /// Prevents the wrapped control from redrawing until the
        /// ResumeDrawing method is called.
        /// </summary>
        public void SuspendDrawing()
        {
            this.Control.SuspendDrawing();
        }

        /// <summary>
        /// Called when a new animation is added to the animation list.
        /// 
        /// Sets up appropriate animation event handlers.
        /// </summary>
        /// 
        /// <param name="addedAnimation">
        /// the animation that was added to the animation list.
        /// </param>
        protected override void OnAnimationAdded(IAnimation addedAnimation)
        {
            base.OnAnimationAdded(addedAnimation);

            // In order to prevent unnecessary redraws, the SuspendRedrawing method needs to
            // be called before the first animated value is set in order to prevent the control
            // from redrawing every time a property is set.
            addedAnimation.AnimationPreIncrement += (animation, _) =>
            {
                if (this.Animations.Length > 0 && 
                    animation == this.Animations.First() &&
                    this.Control.IsHandleCreated)
                    this.Control.BeginInvoke(new Action(this.SuspendDrawing));
            };

            // Once the last property is set, the control is ready to be redrawn.
            addedAnimation.AnimationPostIncrement += (animation, _) =>
            {
                if (this.Animations.Length > 0 && 
                    animation == this.Animations.Last() &&
                    this.Control.IsHandleCreated)
                    this.Control.BeginInvoke(new Action(this.ResumeDrawing));
            };
        }

        /// <summary>
        /// Called when the animations are finished via the Finish method.
        /// </summary>
        protected override void OnAnimationsFinished()
        {
            base.OnAnimationsFinished();

            // See explaination in the OnAnimationStopped method.
            if (!this.Control.InvokeRequired)
                this.Control.Invoke(new Action(() => { }));
        }

        /// <summary>
        /// Called when all animations are stopped via the Stop method.
        /// </summary>
        protected override void OnAnimationsStopped()
        {
            base.OnAnimationsStopped();

            // Ensure the control is not disposed.
            if (this.Control.Disposing || this.Control.IsDisposed)
                return;

            // If the Stop method is called on the UI thread, a potential
            // race condition can occur due to properties being updated
            // asynchronously when the animations are incremented. The
            // race condition occurs with this sequence:
            // 
            // 1. The Animator's thread begins processing an animation frame.
            //    This prevents any animations from being stopped until the
            //    animation frame is finished processing.
            //
            // 2. The Stop method is called on the UI thread before the Animator
            //    increments one of this control's property animations. The Stop
            //    method will block until the Animator finishes processing its
            //    animation frame. Once the control's property animations are
            //    incremented, their properties will be queued to be updated
            //    on the UI thread. This means that the properties will not be
            //    updated until whatever method that called the Stop method
            //    is finished.
            //
            // 3. The method that called the Stop method on the UI thread tries
            //    to use one of the control's properties. Since the control's
            //    properties can't be updated until the method is finished, the
            //    method is essentially guaranteed to be using an out of date
            //    value.
            //
            // This issue can be alleviated by forcing an invoke if the Stop method
            // was called on the UI thread. This will force the UI thread to process
            // the messages that will update the control's proeprties before this
            // method can be returned, ensuring the values will be up to date in
            // case they are used.
            if (!this.Control.InvokeRequired)
                this.Control.Invoke(new Action(() => { }));
        }
    }
}

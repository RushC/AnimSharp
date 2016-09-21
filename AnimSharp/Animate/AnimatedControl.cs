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

        /// <summary>
        /// Allows the wrapped control to redraw after the SuspendDrawing
        /// method was called on it.
        /// </summary>
        public void ResumeDrawing()
        {
            this.Control.ResumeDrawing();
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

            // Ensure that the events for the animation are only fired from
            // the thread that constructed the control.
            addedAnimation.SynchronizationObject = this.Control;

            // In order to prevent unnecessary redraws, the SuspendRedrawing method needs to
            // be called before the first animated value is set in order to prevent the control
            // from redrawing every time a property is set.
            addedAnimation.AnimationPreIncrement += (animation, _) =>
            {
                if (animation == this.Animations.FirstOrDefault() &&
                    this.Control.IsHandleCreated)
                    this.SuspendDrawing();
            };

            // Once the last property is set, the control is ready to be redrawn.
            addedAnimation.AnimationPostIncrement += (animation, _) =>
            {
                if (animation == this.Animations.LastOrDefault() &&
                    this.Control.IsHandleCreated)
                    this.ResumeDrawing();
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

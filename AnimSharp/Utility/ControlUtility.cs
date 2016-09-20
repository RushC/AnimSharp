using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AnimSharp.Utility
{
    /// <summary>
    /// The ControlUtility contains utility methods used to aid
    /// Control animations.
    /// </summary>
    public static class ControlUtility
    {
        // Underlying win32 method for sending a message.
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);

        // ID for the WM_SETREDRAW message for win32.
        private const int WM_SETREDRAW = 11;

        /// <summary>
        /// Resumes redrawing for the specified control.
        /// 
        /// Once the redrawing is resumed, the control will immediately be redrawn.
        /// Any changes made to the control's properties while the redrawing was
        /// suspended will immediately take effect.
        /// 
        /// This method has no reason to be called unless a call to SuspendDrawing
        /// is made first.
        /// </summary>
        /// 
        /// <param name="control">
        /// the control to resume drawing for.
        /// </param>
        public static void ResumeDrawing(this Control control)
        {
            SendMessage(control.Handle, WM_SETREDRAW, true, 0);

            if (control.Parent != null)
                control.Parent.Refresh();
            else
                control.Refresh();
        }

        /// <summary>
        /// Suspends redrawing for the specified control.
        /// 
        /// When the redrawing for a control is suspended, invalidating/refreshing the
        /// control will not work. This can be useful if a complex change to the control's
        /// layout must be made, as it can prevent unnecessary intermediate redraws.
        /// 
        /// To resume redrawing for the specifed control, use the ResumeDrawing method.
        /// </summary>
        /// 
        /// <param name="control">
        /// the control to suspend drawing for.
        /// </param>
        public static void SuspendDrawing(this Control control)
        {
            SendMessage(control.Handle, WM_SETREDRAW, false, 0);
        }
    }
}

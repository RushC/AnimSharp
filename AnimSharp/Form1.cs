using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AnimSharp.Animate;
using AnimSharp.Interpolate;

namespace AnimSharp
{
    internal partial class Form1 : Form
    {
        private AnimatedControl a;
        public static int startx;

        public Form1()
        {
            InitializeComponent();
            a = new AnimatedControl(panel1);
            startx = panel1.Left;
            Animator.Instance.Framerate = 60;
            this.Focus();
        }

        bool to = true;
        private void Start_Click(object sender, EventArgs e)
        {
            a.Stop();

            var b = to ? 650 : 23;
            a.AnimatePropertyTo("Left", b, 1000, Interpolators.Linear);
            to = !to;
            
            //a.AnimatePropertyFrom("Width", 500, 5000, Interpolators.Linear);
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            a.Stop();
        }

        private void Finish_Click(object sender, EventArgs e)
        {
            a.Finish();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S)
                Start.PerformClick();
            else if (e.KeyCode == Keys.F)
                Finish.PerformClick();
        }
    }
}

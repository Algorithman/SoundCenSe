// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: ToolStripSignal.cs
// 
// Last modified: 2016-07-17 22:06

using System.Drawing;
using System.Windows.Forms;

namespace SoundCenSe.GUI
{
    public class ToolStripSignal : ToolStripStatusLabel
    {
        #region Fields and Constants

        private bool signal;

        #endregion

        #region Properties

        public bool Signal
        {
            get { return signal; }
            set
            {
                if (value != signal)
                {
                    signal = value;
                    this.Invalidate();
                }
            }
        }

        #endregion

        public ToolStripSignal()
        {
            this.Text = "";
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Color c = Color.Red;
            if (signal)
            {
                c = Color.Green;
            }
            using (Brush b = new SolidBrush(c))
            {
                e.Graphics.FillEllipse(b, 2, 2, this.Height - 4, this.Height - 4);
            }
        }
    }
}
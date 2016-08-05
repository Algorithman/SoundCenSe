// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: ToolStripSignal.cs
// 
// Last modified: 2016-07-30 19:37

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
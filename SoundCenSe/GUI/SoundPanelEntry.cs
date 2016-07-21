// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SoundPanelEntry.cs
// 
// Last modified: 2016-07-18 20:14

using System;
using System.Drawing;
using System.Windows.Forms;
using SoundCenSe.Events;

namespace SoundCenSe.GUI
{
    public partial class SoundPanelEntry : UserControl
    {
        #region Fields and Constants

        private string channelName = "";
        private DateTime endTime;

        public EventHandler<ChannelFastForwardEventArgs> FastForward;
        private string filename = "";
        private bool isSfxPanel;
        private int length;
        public EventHandler<ChannelMuteEventArgs> Muting;
        public EventHandler<ChannelVolumeEventArgs> VolumeChanged;

        #endregion

        #region Properties

        public string ChannelName
        {
            get { return channelName; }
            set
            {
                labelChannel.Text = value;
                channelName = value;
            }
        }

        public string Filename
        {
            get { return filename; }
            set
            {
                labelFile.Text = value;
                filename = value;
            }
        }

        public bool IsSFXPanel
        {
            get { return isSfxPanel; }
            set
            {
                isSfxPanel = value;
                btnFastForward.Visible = !isSfxPanel;
            }
        }

        public int Length
        {
            get { return length; }
            set
            {
                length = value;
                endTime = DateTime.UtcNow + TimeSpan.FromMilliseconds(length);
                labelLength.Text = (endTime - DateTime.UtcNow).ToString("g");
            }
        }

        public void AddEntry(string filename)
        {
            if (!string.IsNullOrEmpty(filename.Trim()))
            {
                SoundDisabler sd = new SoundDisabler(filename);
                sd.Width = tablePanel.Width;
                sd.SoundDisabled+=SoundDisabledInternal;
                sd.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                AddInFront(sd);
            }
        }

        public EventHandler<DisableSoundEventArgs> SoundDisabled;

        private void SoundDisabledInternal(object sender, DisableSoundEventArgs disableSoundEventArgs)
        {
            var handler = SoundDisabled;
            if (handler != null)
            {
                handler(this, disableSoundEventArgs);
            }
        }

        #endregion

        public SoundPanelEntry()
        {
            InitializeComponent();
            
        }

        private void AddInFront(SoundDisabler sd)
        {
            RowStyle rs = new RowStyle(SizeType.AutoSize);
            tablePanel.RowStyles.Insert(0, rs);
            tablePanel.RowCount++;
            for (int i = tablePanel.Controls.Count - 1; i >= 0; i--)
            {
                tablePanel.SetRow(tablePanel.Controls[i],i+1); 
            }

            tablePanel.Controls.Add(sd, 0, 0);


            sd.BringToFront();
            if (tablePanel.Controls.Count > 5)
            {
                foreach (Control control in tablePanel.Controls)
                {
                    if (tablePanel.GetRow(control) >= 5)
                    {
                        tablePanel.Controls.Remove(control);
                    }
                }
                tablePanel.RowCount--;
            }
        }

        private void FastForwardClick(object sender, EventArgs e)
        {
            var handler = FastForward;
            if (handler != null)
            {
                handler(this, new ChannelFastForwardEventArgs(channelName));
            }
        }

        private void MuteClick(object sender, EventArgs e)
        {
            var handler = Muting;
            if (handler != null)
            {
                handler(this, new ChannelMuteEventArgs(channelName, btnMute.Checked));
            }
        }

        public void Tick()
        {
            if (endTime > DateTime.UtcNow)
            {
                labelLength.Text = (endTime - DateTime.UtcNow).ToString("mm\\:ss\\.f");
            }
            else
            {
                labelLength.Text = "";
                labelFile.Text = "";
            }
        }

        private void VolumeBarValueChanged(object sender, EventArgs e)
        {
            var handler = VolumeChanged;
            if (handler != null)
            {
                handler(this, new ChannelVolumeEventArgs(channelName, VolumeBar.Value/100.0f));
            }
        }

        private bool isDown = false;

        private void button1_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            if (!isDown)
            {
                btnDwnUp.ImageIndex = 1;
                this.Height = 226;
            }
            else
            {
                btnDwnUp.ImageIndex = 0;
                this.Height = 121;
            }
            this.ResumeLayout(true);
            isDown = !isDown;
        }
    }
}
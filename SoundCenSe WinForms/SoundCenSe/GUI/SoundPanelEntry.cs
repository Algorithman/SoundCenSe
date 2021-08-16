// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SoundPanelEntry.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.IO;
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

        private bool isDown;
        private bool isSfxPanel;
        private uint length;
        public EventHandler<ChannelMuteEventArgs> Muting;

        public EventHandler<DisableSoundEventArgs> SoundDisabled;
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
                labelFile.Text = Path.GetFileNameWithoutExtension(value);
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

        public uint Length
        {
            get { return length; }
            set
            {
                length = value;
                endTime = DateTime.UtcNow + TimeSpan.FromMilliseconds(length);
                labelLength.Text = (endTime - DateTime.UtcNow).ToString("g");
            }
        }

        #endregion

        public SoundPanelEntry()
        {
            InitializeComponent();
        }

        public void AddEntry(string filename)
        {
            if (!string.IsNullOrEmpty(filename.Trim()))
            {
                SoundDisabler sd = new SoundDisabler(filename);
                sd.Width = tablePanel.Width;
                sd.SoundDisabled += SoundDisabledInternal;
                sd.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                AddInFront(sd);
            }
        }

        private void AddInFront(SoundDisabler sd)
        {
            tablePanel.RowStyles.Insert(0, new RowStyle(SizeType.AutoSize));
            tablePanel.RowCount++;
            for (int i = tablePanel.Controls.Count - 1; i >= 0; i--)
            {
                tablePanel.SetRow(tablePanel.Controls[i], i + 1);
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
                        control.Dispose();
                    }
                }
                tablePanel.RowCount--;
            }
        }

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
            isDown = !isDown;
            this.ResumeLayout(true);
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

        private void SoundDisabledInternal(object sender, DisableSoundEventArgs disableSoundEventArgs)
        {
            var handler = SoundDisabled;
            if (handler != null)
            {
                handler(this, disableSoundEventArgs);
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
    }
}
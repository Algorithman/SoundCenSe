// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: SoundPanelEntry.cs
// 
// Last modified: 2016-07-17 22:06

using System;
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

        #endregion

        public SoundPanelEntry()
        {
            InitializeComponent();
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
                this.Parent.Controls.Remove(this);
                SoundPanel x = (SoundPanel) this.Parent;
                if (x != null)
                {
                    x.RemoveAndResort(channelName);
                }
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
// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: SoundPanel.cs
// 
// Last modified: 2016-07-17 22:06

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SoundCenSe.Events;

namespace SoundCenSe.GUI
{
    public partial class SoundPanel : UserControl
    {
        #region Fields and Constants

        internal List<string> channelSort = new List<string>();

        public EventHandler<ChannelFastForwardEventArgs> FastForward;
        public EventHandler<ChannelMuteEventArgs> Muting;
        public EventHandler<ChannelVolumeEventArgs> VolumeChanged;

        #endregion

        public SoundPanel()
        {
            InitializeComponent();
        }

        public void Clear()
        {
            foreach (SoundPanelEntry se in this.Controls.OfType<SoundPanelEntry>())
            {
                this.Controls.Remove(se);
            }
        }

        private void FastForwardInternal(object sender, ChannelFastForwardEventArgs channelFastForwardEventArgs)
        {
            var handler = FastForward;
            if (handler != null)
            {
                handler(sender, channelFastForwardEventArgs);
            }
        }

        private int GetPosition(string channel)
        {
            for (int i = 0; i < channelSort.Count; i++)
            {
                if (channelSort.ElementAt(i) == channel)
                {
                    return i;
                }
            }
            throw new Exception("Value not in Set");
        }

        private void MutingInternal(object sender, ChannelMuteEventArgs channelMuteEventArgs)
        {
            var handler = Muting;
            if (handler != null)
            {
                handler(sender, channelMuteEventArgs);
            }
        }

        public void RemoveAndResort(string channelName)
        {
            channelSort.Remove(channelName);
            channelSort = channelSort.OrderBy(x => (x.StartsWith("SFX") ? "Z" : "A") + x).ToList();
        }

        public void SetValues(string channel, string file, int length, bool mute, float volume)
        {
            SoundPanelEntry spe = this.Controls.OfType<SoundPanelEntry>().FirstOrDefault(x => x.ChannelName == channel);
            if (spe == null)
            {
                this.SuspendLayout();
                channelSort.Add(channel);
                channelSort = channelSort.OrderBy(x => (x.StartsWith("SFX") ? "Z" : "A") + x).ToList();
                // Create new one
                spe = new SoundPanelEntry();
                spe.btnMute.Checked = mute;
                int order = GetPosition(channel);
                spe.Location = new Point(0, order*spe.Height);
                spe.Width = this.Width;
                spe.ChannelName = channel;
                spe.Filename = file;
                spe.Length = length;
                spe.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                spe.VolumeBar.Value = (int) (volume*100);
                spe.btnFastForward.Visible = !channel.ToLower().StartsWith("sfx");

                foreach (SoundPanelEntry toOrder in this.Controls.OfType<SoundPanelEntry>())
                {
                    toOrder.Top = GetPosition(toOrder.ChannelName)*toOrder.Height;
                }
                spe.FastForward += FastForwardInternal;
                spe.Muting += MutingInternal;
                spe.VolumeChanged += VolumeChangedInternal;
                this.Controls.Add(spe);
                this.ResumeLayout(true);
            }
            else
            {
                if (spe.Filename != file)
                {
                    spe.ChannelName = channel;
                    spe.Filename = file;
                    spe.Length = length;
                }
            }
        }

        private void SoundPanel_Resize(object sender, EventArgs e)
        {
        }

        public void Tick()
        {
            int count = this.Controls.OfType<SoundPanelEntry>().Count();
            foreach (SoundPanelEntry s in this.Controls.OfType<SoundPanelEntry>())
            {
                s.Tick();
            }
            List<SoundPanelEntry> entries = this.Controls.OfType<SoundPanelEntry>().ToList();

            if (count != entries.Count)
            {
                channelSort.Clear();
                foreach (SoundPanelEntry toOrder in entries)
                {
                    channelSort.Add(toOrder.ChannelName);
                }

                channelSort = channelSort.OrderBy(x => (x.StartsWith("SFX") ? "Z" : "A") + x).ToList();

                foreach (SoundPanelEntry toOrder in entries)
                {
                    toOrder.Top = GetPosition(toOrder.ChannelName)*toOrder.Height;
                }
            }
        }

        private void VolumeChangedInternal(object sender, ChannelVolumeEventArgs channelVolumeEventArgs)
        {
            var handler = VolumeChanged;
            if (handler != null)
            {
                handler(sender, channelVolumeEventArgs);
            }
        }
    }
}
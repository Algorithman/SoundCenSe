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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SoundCenSe.Configuration;
using SoundCenSe.Events;
using SoundCenSe.Utility;

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
            tablePanel.Controls.Clear();
        }

        private void FastForwardInternal(object sender, ChannelFastForwardEventArgs channelFastForwardEventArgs)
        {
            var handler = FastForward;
            if (handler != null)
            {
                handler(sender, channelFastForwardEventArgs);
            }
        }


        public void FillEntries(List<string> channelNames)
        {
            int rowCount = 0;
            foreach (string s in channelNames)
            {
                SoundPanelEntry spe = new SoundPanelEntry();
                spe.ChannelName = s.Capitalize();
                spe.Dock = DockStyle.Fill;
                // spe.Width = this.tablePanel.Width;
                if (s.ToLower() == "sfx")
                {
                    spe.IsSFXPanel = true;
                }
                ChannelData cd = Config.Instance.Channels.FirstOrDefault(x => x.Channel == s.ToLower());
                if (cd != null)
                {
                    spe.VolumeBar.Value = (int)(cd.Volume * 100);
                    spe.btnMute.Checked = cd.Mute;
                }


                spe.FastForward += FastForwardInternal;
                spe.Muting += MutingInternal;
                spe.VolumeChanged += VolumeChangedInternal;
                spe.SoundDisabled += SoundDisabledInternal;
                spe.Anchor = AnchorStyles.Left | AnchorStyles.Right;

                this.tablePanel.Controls.Add(spe, 0, rowCount++);
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


        private void MutingInternal(object sender, ChannelMuteEventArgs channelMuteEventArgs)
        {
            var handler = Muting;
            if (handler != null)
            {
                handler(sender, channelMuteEventArgs);
            }
        }


        public void SetValues(string channel, string file, int length, bool mute, float volume)
        {
            SoundPanelEntry spe = this.tablePanel.Controls.OfType<SoundPanelEntry>().FirstOrDefault(x => x.ChannelName == channel);
            if (spe == null)
            {
                throw new Exception("Channel not initialized yet? (" + channel + ")");
            }
            else
            {
                spe.VolumeBar.Value = (int)(volume * 100);
                spe.btnMute.Checked = mute;
                if (spe.Filename != file)
                {
                    spe.ChannelName = channel;
                    spe.Filename = file;
                    spe.Length = length;
                }
                spe.AddEntry(file);
            }
        }

        public void Tick()
        {
            foreach (SoundPanelEntry s in this.tablePanel.Controls.OfType<SoundPanelEntry>())
            {
                s.Tick();
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
using System;
using System.IO;
using System.Collections.Generic;
using Misc;

namespace SoundCenSeGUI
{
    [System.ComponentModel.ToolboxItem(true)]
    public partial class SoundPanelEntry : Gtk.Bin
    {
        private string channelName = "";
        private uint length;
        private DateTime endTime;
        private double volume = 1.0f;
        private List<SoundDisabler> disablers = new List<SoundDisabler>();

        public SoundPanelEntry(string channel)
        {
            this.Build();
            ChannelName = channel;
            ShowAll();
            btnFastForward.Visible = false;
        }

        public string ChannelName
        {
            get{ return channelName; }
            set
            {
                channelName = value;
                labelChannel.Text = value;
            }
        }

        private string filename = "";

        public string Filename
        {
            get { return filename; }
            set
            {
                labelFile.Text = System.IO.Path.GetFileNameWithoutExtension(value);
                filename = value;
            }
        }

        public uint Length
        {
            get { return length; }
            set
            {
                length = value;
                endTime = DateTime.UtcNow + TimeSpan.FromMilliseconds(length);
                labelRemaining.Text = (endTime - DateTime.UtcNow).ToString("g");
            }
        }

        public double Volume
        {
            get{ return volume; }
            set
            {
                volumeScale.Value = value;
                volume = value;
            }
        }

        public void AddEntry(string filename)
        {
            if (!string.IsNullOrEmpty(filename.Trim()))
            {
                SoundDisabler sd = new SoundDisabler(filename.Trim());
                sd.SoundDisabled += SoundDisabledInternal;
                disablers.Add(sd);
                DisablerTable.PackEnd(sd);
                sd.Show();
                if (disablers.Count > 5)
                {
                    SoundDisabler toRemove = disablers[0];
                    disablers.RemoveAt(0);
                    toRemove.Destroy();
                }
            }
        }

        public EventHandler<DisableSoundEventArgs> SoundDisabled;

        private void SoundDisabledInternal(object sender, DisableSoundEventArgs e)
        {
            var handler = SoundDisabled;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        protected void btnFastForwardClicked(object sender, EventArgs e)
        {
            OnFastForward();
        }

        public EventHandler<ChannelFastForwardEventArgs> ChannelFastForward;

        protected virtual void OnFastForward()
        {
            var handler = ChannelFastForward;
            if (handler != null)
            {
                handler(this, new ChannelFastForwardEventArgs(channelName));
            }
        }

        public EventHandler<ChannelMuteEventArgs> ChannelMute;

        protected virtual void OnChannelMute()
        {
            var handler = ChannelMute;
            if (handler != null)
            {
                handler(this, new ChannelMuteEventArgs(channelName, btnMute.Active));
            }
        }

        protected void btnChannelMuteClick(object sender, EventArgs e)
        {
            OnChannelMute();
        }

        public void Tick()
        {
            if (endTime > DateTime.UtcNow)
            {
                labelRemaining.Text = (endTime - DateTime.UtcNow).ToString("mm\\:ss\\.f");
            }
            else
            {
                labelRemaining.Text = "";
                labelFile.Text = "";
            }
        }


        public EventHandler<VolumeChangedEventArgs> VolumeChanged;

        protected virtual void OnVolumeChanged()
        {
            double value = volumeScale.Value;
            volume = value;

            var handler = VolumeChanged;
            if (handler != null)
            {
                handler(this, new VolumeChangedEventArgs(channelName, value));
            }
        }

        private bool mute = false;

        public bool Mute
        {
            get{ return mute; }
            set
            {
                mute = value;
                btnMute.Active = value;
            }
        }

        public void SetValues(string filename, uint soundLength, bool muted, double volume, bool looping)
        {
            Gtk.Application.Invoke(delegate
            {
                Filename = filename;
                Length = soundLength;
                Volume = volume;
                Mute = muted;
                btnFastForward.Visible = looping;
                AddEntry(filename);
            });
        }

        protected void btnMuteClicked(object sender, EventArgs e)
        {
            OnChannelMute();
        }

        protected void SliderMoved(object sender, EventArgs e)
        {
            OnVolumeChanged();
        }
    }
}


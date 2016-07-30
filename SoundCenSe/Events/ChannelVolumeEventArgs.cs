// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: ChannelVolumeEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

using System;

namespace SoundCenSe.Events
{
    public class ChannelVolumeEventArgs : EventArgs
    {
        #region Properties

        public string Channel { get; set; }
        public float Volume { get; set; }

        #endregion

        public ChannelVolumeEventArgs(string channel, float volume)
        {
            Channel = channel;
            Volume = volume;
        }
    }
}
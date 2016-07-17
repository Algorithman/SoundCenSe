// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: ChannelVolumeEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

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
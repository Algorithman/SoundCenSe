// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: ChannelMuteEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

using System;

namespace SoundCenSe.Events
{
    public class ChannelMuteEventArgs : EventArgs
    {
        #region Properties

        public string Channel { get; set; }

        public bool Mute { get; set; }

        #endregion

        public ChannelMuteEventArgs(string channel, bool mute)
        {
            Channel = channel;
            Mute = mute;
        }
    }
}
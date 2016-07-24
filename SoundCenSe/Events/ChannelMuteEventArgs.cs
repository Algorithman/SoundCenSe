// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: ChannelMuteEventArgs.cs
// 
// Last modified: 2016-07-24 13:52

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
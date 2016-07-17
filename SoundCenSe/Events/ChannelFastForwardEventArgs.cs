// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: ChannelFastForwardEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

using System;

namespace SoundCenSe.Events
{
    public class ChannelFastForwardEventArgs : EventArgs
    {
        #region Properties

        public string Channel { get; set; }

        #endregion

        public ChannelFastForwardEventArgs(string channel)
        {
            Channel = channel;
        }
    }
}
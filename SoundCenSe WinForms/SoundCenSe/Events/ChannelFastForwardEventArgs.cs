// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: ChannelFastForwardEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

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
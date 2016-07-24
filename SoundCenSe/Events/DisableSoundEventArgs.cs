// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: DisableSoundEventArgs.cs
// 
// Last modified: 2016-07-24 13:52

using System;

namespace SoundCenSe.Events
{
    public class DisableSoundEventArgs : EventArgs
    {
        #region Properties

        public bool Disable { get; set; }
        public string Filename { get; set; }

        #endregion

        public DisableSoundEventArgs(string filename, bool disable)
        {
            Filename = filename;
            Disable = disable;
        }
    }
}
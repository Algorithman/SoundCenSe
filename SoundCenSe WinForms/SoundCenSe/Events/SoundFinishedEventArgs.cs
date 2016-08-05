// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SoundFinishedEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using SoundCenSe.Configuration.Sounds;

namespace SoundCenSe.Events
{
    public class SoundFinishedEventArgs : EventArgs
    {
        #region Properties

        public Sound Sound { get; set; }
        public SoundFile SoundFile { get; set; }

        #endregion

        public SoundFinishedEventArgs(Sound sound, SoundFile soundFile)
        {
            Sound = sound;
            SoundFile = soundFile;
        }
    }
}
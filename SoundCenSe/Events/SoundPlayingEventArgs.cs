// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: SoundPlayingEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

using System;
using SoundCenSe.Configuration.Sounds;

namespace SoundCenSe.Events
{
    public class SoundPlayingEventArgs : EventArgs
    {
        #region Properties

        public bool Mute { get; set; }

        public Sound Sound { get; set; }
        public SoundFile SoundFile { get; set; }
        public float Volume { get; set; }

        #endregion

        public SoundPlayingEventArgs(Sound sound, SoundFile soundFile, bool mute, float volume)
        {
            Sound = sound;
            SoundFile = soundFile;
            Mute = mute;
            Volume = volume;
        }
    }
}
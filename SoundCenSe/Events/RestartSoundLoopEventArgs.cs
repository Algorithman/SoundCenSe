// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: RestartSoundLoopEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using SoundCenSe.fmodInternal;

namespace SoundCenSe.Events
{
    public class RestartSoundLoopEventArgs : EventArgs
    {
        #region Properties

        public SoundSoundFile SoundSoundFile { get; set; }

        #endregion

        public RestartSoundLoopEventArgs(SoundSoundFile soundSoundFile)
        {
            SoundSoundFile = soundSoundFile;
        }
    }
}
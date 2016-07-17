// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: UpdateFinishedEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

using System;

namespace SoundCenSe.Events
{
    public class UpdateFinishedEventArgs : EventArgs
    {
        #region Properties

        public bool ReloadNecessary { get; set; }

        #endregion

        public UpdateFinishedEventArgs(bool reload)
        {
            ReloadNecessary = reload;
        }
    }
}
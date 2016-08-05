// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: UpdateFinishedEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

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
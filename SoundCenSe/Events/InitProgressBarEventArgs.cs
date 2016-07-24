// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: InitProgressBarEventArgs.cs
// 
// Last modified: 2016-07-24 13:52

using System;

namespace SoundCenSe.Events
{
    public class InitProgressBarEventArgs : EventArgs
    {
        #region Properties

        public int Maximum { get; set; }

        #endregion

        public InitProgressBarEventArgs(int maximum)
        {
            Maximum = maximum;
        }
    }
}
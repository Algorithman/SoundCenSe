// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: InitProgressBarEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

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
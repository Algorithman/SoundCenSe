// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: DwarfFortressRunningEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

using System;

namespace SoundCenSe.Events
{
    public class DwarfFortressRunningEventArgs : EventArgs
    {
        #region Properties

        public int ProcessId { get; set; }

        #endregion

        public DwarfFortressRunningEventArgs(int processId)
        {
            ProcessId = processId;
        }
    }
}
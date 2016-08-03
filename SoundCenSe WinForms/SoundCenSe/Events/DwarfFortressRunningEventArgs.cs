// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: DwarfFortressRunningEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

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
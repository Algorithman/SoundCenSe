// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: GamelogEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

using System;

namespace SoundCenSe.Events
{
    public class GamelogEventArgs : EventArgs
    {
        #region Properties

        public string Line { get; set; }

        #endregion

        public GamelogEventArgs(string line)
        {
            Line = line;
        }
    }
}
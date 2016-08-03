// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: GamelogEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

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
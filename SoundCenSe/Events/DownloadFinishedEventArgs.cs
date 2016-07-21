// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: DownloadFinishedEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

#region Usings

using System;
using SoundCenSe.Utility.Updater;

#endregion

namespace SoundCenSe.Events
{
    public class DownloadFinishedEventArgs : EventArgs
    {
        #region Properties

        public DownloadEntry File { get; set; }

        #endregion

        public DownloadFinishedEventArgs(DownloadEntry file)
        {
            File = file;
        }
    }
}
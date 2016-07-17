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

#endregion

namespace SoundCenSe.Utility.Updater
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
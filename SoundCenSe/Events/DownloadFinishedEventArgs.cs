// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: DownloadFinishedEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

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
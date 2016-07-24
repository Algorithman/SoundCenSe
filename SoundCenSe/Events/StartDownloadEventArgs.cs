// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: StartDownloadEventArgs.cs
// 
// Last modified: 2016-07-24 13:52

using System;
using SoundCenSe.Utility.Updater;

namespace SoundCenSe.Events
{
    public class StartDownloadEventArgs : EventArgs
    {
        #region Properties

        public DownloadEntry File { get; set; }

        #endregion

        public StartDownloadEventArgs(DownloadEntry file)
        {
            File = file;
        }
    }
}
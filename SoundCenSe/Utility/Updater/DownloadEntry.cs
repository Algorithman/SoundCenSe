// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: DownloadEntry.cs
// 
// Last modified: 2016-07-24 13:52

#region Usings

using System;
using System.IO;
using System.Net;
using SoundCenSe.Events;

#endregion

namespace SoundCenSe.Utility.Updater
{
    public class DownloadEntry : IDisposable
    {
        #region Fields and Constants

        public EventHandler<DownloadFinishedEventArgs> FinishedFile;

        #endregion

        #region Properties

        public string DestinationPath { get; set; }
        public string ExpectedSHA { get; set; }
        public long ExpectedSize { get; set; }
        public DownloadResult Result { get; set; }

        public string SourceURL { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string TempFileName { get; set; }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (File.Exists(TempFileName))
                {
                    File.Delete(TempFileName);
                }
            }
        }
    }
}
using System;
using Misc;
using System.Net;
using System.IO;

namespace SoundCenSeGTK
{
    public class DownloadEntry : IDownloadEntry, IDisposable
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

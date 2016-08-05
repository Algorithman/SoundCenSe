using System;

namespace Misc
{

    public class StartDownloadEventArgs : EventArgs
    {
        #region Properties

        public IDownloadEntry File { get; set; }

        #endregion

        public StartDownloadEventArgs(IDownloadEntry file)
        {
            File = file;
        }
    }
}


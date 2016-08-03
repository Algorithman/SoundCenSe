using System;

namespace Misc
{
    public class DownloadFinishedEventArgs : EventArgs
    {
        public IDownloadEntry File { get; set; }

        public DownloadFinishedEventArgs(IDownloadEntry file)
        {
            File = file;
        }
    }
}


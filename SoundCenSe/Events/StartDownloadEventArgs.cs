using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

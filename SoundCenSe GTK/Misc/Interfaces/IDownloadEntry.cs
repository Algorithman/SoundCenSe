using System;
using System.Net;

namespace Misc
{
    public interface IDownloadEntry
    {
        string DestinationPath { get; set; }

        string ExpectedSHA { get; set; }

        long ExpectedSize { get; set; }

        DownloadResult Result { get; set; }

        string SourceURL { get; set; }

        HttpStatusCode StatusCode { get; set; }

        string TempFileName { get; set; }
    }
}


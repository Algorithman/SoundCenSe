using System;
using Misc;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using NLog;
using System.Threading;
using System.Threading.Tasks;
using SeasideResearch.LibCurlNet;

namespace SoundCenSeGTK
{
    public class PackDownloader
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const string updateXML = "autoUpdater.xml";

        private bool autoUpdateFileDownloaded;

        public EventHandler<StartDownloadEventArgs> DownloadStarted;

        public EventHandler<DownloadFinishedEventArgs> FinishedFile;

        private bool stop;

        public EventHandler<UpdateFinishedEventArgs> UpdateFinished;

        private List<DirectoryData> UpdateList = new List<DirectoryData>();

        #endregion

        #region Properties

        public List<IDownloadEntry> DownloadedFiles { get; set; }

        private List<IDownloadEntry> FilesInProgress { get; set; }

        private Queue<IDownloadEntry> FilesToDownload { get; set; }

        #endregion

        public PackDownloader()
        {
            DownloadedFiles = new List<IDownloadEntry>();
            FilesToDownload = new Queue<IDownloadEntry>();
            FilesInProgress = new List<IDownloadEntry>();
            FinishedFile += FileDownloaded;
            Curl.GlobalInit((int)CURLinitFlag.CURL_GLOBAL_ALL);
        }

        #region IStoppable Members

        public void Start()
        {
            stop = false;
        }

        public void Stop()
        {
            stop = true;
        }

        #endregion

        public void AddFileForDownload(DownloadEntry file)
        {
            logger.Info("Added file for download: " + file.SourceURL);
            FilesToDownload.Enqueue(file);
        }

        private void CheckAndDeleteFiles()
        {
            string[] files = Directory.GetFiles(Path.GetFullPath(Config.Instance.soundpacksPath), "*.*",
                                 SearchOption.AllDirectories);

            List<string> fullPaths = new List<string>();
            string soundpacksPath = Path.GetFullPath(Config.Instance.soundpacksPath);
            foreach (DirectoryData dd in UpdateList)
            {
                string destPath = Path.Combine(soundpacksPath, dd.RelativePath, dd.Filename)
                        .Replace(
                                      Path.DirectorySeparatorChar + "packs" + Path.DirectorySeparatorChar + "packs",
                                      Path.DirectorySeparatorChar + "packs");
                fullPaths.Add(NormalizePath(Path.GetFullPath(destPath)));
            }

            fullPaths = fullPaths.OrderBy(x => x).ToList();

            foreach (string file in files)
            {
                if (!fullPaths.Contains(NormalizePath(file)))
                {
                    File.Delete(file);
                }
            }
            RemoveEmptyFolders(soundpacksPath);
        }


        public int Count()
        {
            return (int)(FilesToDownload.Sum(x => x.ExpectedSize) + FilesInProgress.Sum(x => x.ExpectedSize));
        }

        public void DoWork()
        {
            if (!stop && (FilesToDownload.Count + FilesInProgress.Count > 0))
            {
                Tick();
            }
            else
            {
                OnUpdateFinished();
            }
        }

        private void FileDownloaded(object sender, DownloadFinishedEventArgs evArgs)
        {
            FilesInProgress.Remove(evArgs.File);
            if (evArgs.File.StatusCode == HttpStatusCode.OK)
            {
                if (string.IsNullOrEmpty(evArgs.File.ExpectedSHA) ||
                    (UpdateParser.SHA1Checksum(evArgs.File.TempFileName) == evArgs.File.ExpectedSHA))
                {
                    if (!string.IsNullOrEmpty(evArgs.File.DestinationPath))
                    {
                        if (File.Exists(evArgs.File.DestinationPath))
                        {
                            File.Delete(evArgs.File.DestinationPath);
                        }
                        if (!Directory.Exists(Path.GetDirectoryName(evArgs.File.DestinationPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(evArgs.File.DestinationPath));
                        }
                        File.Move(evArgs.File.TempFileName, evArgs.File.DestinationPath);
                    }

                    EventHandler<DownloadFinishedEventArgs> handler = ((DownloadEntry)evArgs.File).FinishedFile;
                    if (handler != null)
                    {
                        handler(evArgs.File, new DownloadFinishedEventArgs(evArgs.File));
                    }
                }
                else
                {
                    logger.Error("File " + evArgs.File.SourceURL + " had wrong checksum (" + evArgs.File.ExpectedSHA +
                        "<->" + UpdateParser.SHA1Checksum(evArgs.File.TempFileName));
                    // AddFileForDownload(evArgs.File);
                }
            }
            else
            {
                logger.Error("Download of file " + evArgs.File.SourceURL + " failed with errorcode " +
                    evArgs.File.StatusCode);
            }
        }

        private void FinishedAutoUpdateFile(object sender, DownloadFinishedEventArgs downloadFinishedEventArgs)
        {
            autoUpdateFileDownloaded = true;
        }

        public bool HasQueueEntries()
        {
            return FilesToDownload.Count > 0;
        }

        public bool IsDownloading()
        {
            return FilesInProgress.Count > 0;
        }

        private string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                    .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .ToUpperInvariant();
        }

        private void OnStartDownload(IDownloadEntry file)
        {
            var handler = DownloadStarted;
            if (handler != null)
            {
                handler(this, new StartDownloadEventArgs(file));
            }
        }

        private void OnUpdateFinished()
        {
            if (Config.Instance.deleteFiles)
            {
                CheckAndDeleteFiles();
            }

            var handler = UpdateFinished;
            if (handler != null)
            {
                handler(this, new UpdateFinishedEventArgs(true));
            }
        }

        private void RemoveEmptyFolders(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                RemoveEmptyFolders(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        private void StartDownload(IDownloadEntry file, string fromServer)
        {
            OnStartDownload(file);
            int tries = 5;
            while (tries > 0)
            {
                tries--;
                string dl = fromServer + file.SourceURL;
                try
                {
                    file.StatusCode = HttpStatusCode.OK;
                    try
                    {
                        Easy easydl = new Easy();
                        Easy.WriteFunction wf = (byte[] buf, int size, int nmemb, object extraData) => 
                        {
                            FileStream fs = new FileStream(file.TempFileName, FileMode.OpenOrCreate);
                            fs.Seek(0, SeekOrigin.End);
                            fs.Write(buf, 0, size);
                            fs.Close();
                            return size;
                        };
                        easydl.SetOpt(CURLoption.CURLOPT_URL, Uri.EscapeUriString(dl));
                        easydl.SetOpt(CURLoption.CURLOPT_WRITEFUNCTION, wf);
                        easydl.Perform();
                        easydl.Dispose();
                    }
                    catch (Exception ex)
                    {
                        logger.Warn("Download error: "+ex.Message);
                        logger.Warn(ex.StackTrace);
                    }
                    using (WebClient webClient = new WebClient())
                    {
                        webClient.DownloadFile(Uri.EscapeUriString(dl), file.TempFileName);
                    }
                }
                catch (WebException wbex)
                {
                    logger.Error("WEBEXCEPTION: " + wbex.Status);
                    logger.Error("At file " + dl);
                    try
                    {
                        file.StatusCode = ((HttpWebResponse)wbex.Response).StatusCode;
                    }
                    catch (Exception)
                    {
                        file.StatusCode = HttpStatusCode.RequestTimeout;
                    }
                }

                if (file.StatusCode != HttpStatusCode.OK)
                {
                    fromServer = Config.Instance.autoUpdateURLs.OrderBy(
                        x => (new Random((int)DateTime.Now.Ticks).Next(50000))).First(x => x != fromServer);
                }
                else
                {
                    file.SourceURL = fromServer + file.SourceURL;
                    EventHandler<DownloadFinishedEventArgs> handler = FinishedFile;
                    if (handler != null)
                    {
                        handler(this, new DownloadFinishedEventArgs(file));
                    }
                    break;
                }
            }
        }

        public void Tick()
        {
            while ((FilesToDownload.Count > 0) && (FilesInProgress.Count < 10))
            {
                IDownloadEntry file = FilesToDownload.Dequeue();
                if (string.IsNullOrEmpty(file.TempFileName))
                {
                    file.TempFileName = Path.GetTempFileName();
                }

                FilesInProgress.Add(file);


                string server =
                    Config.Instance.autoUpdateURLs.OrderBy(
                        x => FilesInProgress.Count(y => y.SourceURL.StartsWith(x))).First();

                Task.Factory.StartNew(() => StartDownload(file, server));
            }
        }

        internal void UpdateSoundPack()
        {
            DownloadEntry de = new DownloadEntry
            {
                SourceURL = "/autoUpdater.xml",
                DestinationPath = Path.Combine(Config.Instance.soundpacksPath, "autoUpdater.xml")
            };

            de.FinishedFile += FinishedAutoUpdateFile;

            autoUpdateFileDownloaded = false;
            FilesToDownload.Enqueue(de);
            Tick();
            while (!autoUpdateFileDownloaded)
            {
                Thread.Sleep(1000);
            }

            Random rnd = new Random();
            UpdateList.Clear();

            UpdateList = UpdateParser.Parse(de.DestinationPath).OrderBy(x => rnd.Next(50000)).ToList();

            string soundpacksPath = Path.GetFullPath(Config.Instance.soundpacksPath);
            foreach (DirectoryData dd in UpdateList)
            {
                string destPath = Path.Combine(soundpacksPath, dd.RelativePath, dd.Filename)
                        .Replace(
                                      Path.DirectorySeparatorChar + "packs" + Path.DirectorySeparatorChar + "packs" +
                                      Path.DirectorySeparatorChar,
                                      Path.DirectorySeparatorChar + "packs" + Path.DirectorySeparatorChar);
                if (Config.Instance.replaceFiles || !File.Exists(destPath))
                {
                    string sha1 = UpdateParser.SHA1Checksum(destPath);
                    if (dd.SHA1 != sha1)
                    {
                        DownloadEntry d = new DownloadEntry();
                        d.DestinationPath = destPath;
                        d.SourceURL = "/" + Path.Combine(dd.RelativePath, dd.Filename).Replace("\\", "/");
                        d.SourceURL = d.SourceURL.Replace("/packs/", "/");
                        d.ExpectedSHA = dd.SHA1;
                        d.ExpectedSize = dd.Size;
                        AddFileForDownload(d);
                    }
                }
            }
        }
    }
}

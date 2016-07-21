﻿// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: PackDownloader.cs
// 
// Last modified: 2016-07-21 21:42

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SoundCenSe.Events;
using SoundCenSe.Interfaces;
using SoundCenSe.Utility.Updater.XML;

#endregion

namespace SoundCenSe.Utility.Updater
{
    public class PackDownloader : IStoppable
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private const string updateXML = "autoUpdater.xml";

        private bool autoUpdateFileDownloaded;

        public EventHandler<StartDownloadEventArgs> DownloadStarted;

        public EventHandler<DownloadFinishedEventArgs> FinishedFile;

        private bool stop;

        public EventHandler<UpdateFinishedEventArgs> UpdateFinished;

        #endregion

        #region Properties

        public List<DownloadEntry> DownloadedFiles { get; set; }
        private List<DownloadEntry> FilesInProgress { get; }
        private Queue<DownloadEntry> FilesToDownload { get; }

        #endregion

        public PackDownloader()
        {
            DownloadedFiles = new List<DownloadEntry>();
            FilesToDownload = new Queue<DownloadEntry>();
            FilesInProgress = new List<DownloadEntry>();
            FinishedFile += FileDownloaded;
        }

        #region IStoppable Members

        public void Start()
        {
            stop = false;
            Task.Factory.StartNew(() => DoWork());
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


        public int Count()
        {
            return (int) (FilesToDownload.Sum(x => x.ExpectedSize) + FilesInProgress.Sum(x => x.ExpectedSize));
        }

        private void DoWork()
        {
            while (!stop && (FilesToDownload.Count + FilesInProgress.Count > 0))
            {
                Tick();
                Thread.Sleep(100);
            }

            OnUpdateFinished();
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

                    EventHandler<DownloadFinishedEventArgs> handler = evArgs.File.FinishedFile;
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

        private void OnStartDownload(DownloadEntry file)
        {
            var handler = DownloadStarted;
            if (handler != null)
            {
                handler(this, new StartDownloadEventArgs(file));
            }
        }

        private void OnUpdateFinished()
        {
            var handler = UpdateFinished;
            if (handler != null)
            {
                handler(this, new UpdateFinishedEventArgs(true));
            }
        }

        private void StartDownload(DownloadEntry file, string fromServer)
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
                        file.StatusCode = ((HttpWebResponse) wbex.Response).StatusCode;
                    }
                    catch (Exception)
                    {
                        file.StatusCode = HttpStatusCode.RequestTimeout;
                    }
                }

                if (file.StatusCode != HttpStatusCode.OK)
                {
                    fromServer = Config.Instance.autoUpdateURLs.OrderBy(
                        x => (new Random((int) DateTime.Now.Ticks).Next(50000))).First(x => x != fromServer);
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
                DownloadEntry file = FilesToDownload.Dequeue();
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
            List<DirectoryData> list = UpdateParser.Parse(de.DestinationPath).OrderBy(x => rnd.Next(50000)).ToList();

            foreach (DirectoryData dd in list)
            {
                DownloadEntry d = new DownloadEntry();
                d.DestinationPath =
                    Path.Combine(Config.Instance.soundpacksPath, dd.RelativePath, dd.Filename)
                        .Replace(
                            Path.DirectorySeparatorChar + "packs" + Path.DirectorySeparatorChar + "packs" +
                            Path.DirectorySeparatorChar,
                            Path.DirectorySeparatorChar + "packs" + Path.DirectorySeparatorChar);
                d.SourceURL = "/" + Path.Combine(dd.RelativePath, dd.Filename).Replace("\\", "/");
                d.SourceURL = d.SourceURL.Replace("/packs/", "/");
                d.ExpectedSHA = dd.SHA1;
                d.ExpectedSize = dd.Size;
                AddFileForDownload(d);
            }
        }


        /*
        public string MainPath { get; set; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)
            .Replace("file://", "")
            .Replace("file:\\", "");
            */
    }
}
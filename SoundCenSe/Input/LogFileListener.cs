// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: LogFileListener.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using SoundCenSe.Events;
using SoundCenSe.Interfaces;

namespace SoundCenSe.Input
{
    public sealed class LogFileListener : IStoppable, IDisposable
    {
        #region Fields and Constants

        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public EventHandler<GamelogEventArgs> GamelogEvent;
        private TextReader logfileReader;
        private FileStream logFileStream;
        private bool stop;

        #endregion

        public LogFileListener(string filename, bool beginAtEnd = false)
        {
            logFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            logfileReader = new StreamReader(logFileStream);
            if (beginAtEnd)
            {
                BeginAtEnd();
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region IStoppable Members

        public void Start()
        {
            Task.Factory.StartNew(() => DoWork());
        }

        public void Stop()
        {
            stop = true;
        }

        #endregion

        public void BeginAtEnd()
        {
            logfileReader.ReadToEnd();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Stop();
                logfileReader.Dispose();
                logFileStream.Dispose();
                logfileReader = null;
                logFileStream = null;
            }
        }

        private void DoWork()
        {
            stop = false;
            while (!stop)
            {
                if (logfileReader == null)
                {
                    break;
                }
                string line = logfileReader.ReadLine();
                if (line != null)
                {
                    OnLogfileEvent(line);
                    continue;
                }

                Thread.Sleep(50);
            }
        }

        private void OnLogfileEvent(string line)
        {
            var handler = GamelogEvent;
            if (handler != null)
            {
                handler(this, new GamelogEventArgs(line));
            }
        }
    }
}
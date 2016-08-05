using System;
using NLog;
using Misc;
using System.IO;
using System.Text.RegularExpressions;

namespace SoundCenSeGTK
{
    public sealed class LogFileListener : IDisposable
    {
        #region Fields and Constants

        public static readonly Logger logger = LogManager.GetCurrentClassLogger();
        public EventHandler<GamelogEventArgs> GamelogEvent;
        private TextReader logfileReader;
        private FileStream logFileStream;

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
        }

        public void Stop()
        {
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

        public void Tick()
        {
            if (logfileReader == null)
            {
                return;
            }
            while (true)
            {
                try
                {
                    string line = logfileReader.ReadLine();
                    if (line != null)
                    {
                        OnLogfileEvent(line);
                    }
                    else
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Exception while reading gamelog");
                    logger.Error(ex);
                }
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


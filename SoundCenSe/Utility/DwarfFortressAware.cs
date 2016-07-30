// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: DwarfFortressAware.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SoundCenSe.Events;
using SoundCenSe.Interfaces;

namespace SoundCenSe.Utility
{
    public class DwarfFortressAware : IStoppable
    {
        #region Fields and Constants

        private int dwarfFortressProcessId = -1;
        private string dwarfFortressProcessName;
        private string dwarfFortressProcessPath;
        public EventHandler<DwarfFortressRunningEventArgs> DwarfFortressRunning;
        public EventHandler<DwarfFortressStoppedEventArgs> DwarfFortressStopped;
        private bool stop;

        #endregion

        #region Properties

        public string GameLogPath { get; set; }

        #endregion

        #region IStoppable Members

        public void Start()
        {
#if __MonoCS__
            GameLogPath = Config.Instance.gamelogPath;
            OnDwarfFortressRunning(0);
#else
            GameLogPath = "";
            Task.Factory.StartNew(() => DoWork());
#endif
        }

        public void Stop()
        {
            stop = true;
        }

        #endregion

        private int ran;

        private void DoWork()
        {
            stop = false;
            while (!stop)
            {
                if (dwarfFortressProcessId == -1)
                {
                    // DF not found yet
                    string processName = null;
                    string processPath = null;
                    try
                    {
                        int processId = GetDwarfFortressProcessId(out processName, out processPath);
                        if (processId != -1)
                        {
                            dwarfFortressProcessId = processId;
                            dwarfFortressProcessName = processName;
                            dwarfFortressProcessPath = processPath;
                            ran++;
                            OnDwarfFortressRunning(processId);
                        }
                    }
                    catch (Exception)
                    {
                        // In case Process stops while reading data
                        dwarfFortressProcessId = -1;
                        dwarfFortressProcessName = null;
                        dwarfFortressProcessPath = null;
                    }
                }
                else
                {
                    Process p = null;
                    try
                    {
                        p = Process.GetProcessById(dwarfFortressProcessId);
                    }
                    catch (ArgumentException)
                    {
                    }

                    string processPath = "";
                    string processName = "";
                    try
                    {
                        processPath = Path.GetDirectoryName(p.MainModule.FileName);
                        processName = p.ProcessName;
                    }
                    catch (Exception)
                    {
                        // In case Process stops while reading data
                    }

                    if ((p == null) || (processName != dwarfFortressProcessName) ||
                        (processPath != dwarfFortressProcessPath))
                    {
                        dwarfFortressProcessId = -1;
                        dwarfFortressProcessName = null;
                        dwarfFortressProcessPath = null;
                        OnDwarfFortressStopped();
                    }
                }
                Thread.Sleep(1000);
            }
            dwarfFortressProcessId = -1;
            dwarfFortressProcessName = null;
            dwarfFortressProcessPath = null;
        }

#if __MonoCS__
        const string DFName = "Dwarf_Fortress";
        const string RelPathGamelog = "../gamelog.txt";
#else
        const string DFName = "Dwarf Fortress";
        const string RelPathGamelog = "gamelog.txt";
#endif


        private int GetDwarfFortressProcessId(out string processName, out string processPath)
        {
            List<Process> df = Process.GetProcessesByName(DFName).ToList();
            foreach (Process p in df)
            {
                string path = Path.GetDirectoryName(p.MainModule.FileName);

                if (!File.Exists(Path.Combine(path, RelPathGamelog)))
                {
                    continue;
                }

                GameLogPath = Path.Combine(Path.GetDirectoryName(p.MainModule.FileName), RelPathGamelog);
                processName = p.ProcessName;
                processPath = path;
                return p.Id;
            }
            processName = null;
            processPath = null;
            return -1;
        }

        protected void OnDwarfFortressRunning(int processId)
        {
            var handler = DwarfFortressRunning;
            if (handler != null)
            {
                handler(this, new DwarfFortressRunningEventArgs(processId));
            }
        }

        protected void OnDwarfFortressStopped()
        {
            var handler = DwarfFortressStopped;
            if (handler != null)
            {
                handler(this, new DwarfFortressStoppedEventArgs());
            }
        }
    }
}
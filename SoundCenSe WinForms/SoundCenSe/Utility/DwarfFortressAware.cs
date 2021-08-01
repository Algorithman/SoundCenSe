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
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using NLog;
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

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
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
                    catch (Exception ex)
                    {
                        logger.Trace(ex, "Failed to attach to gamelog.txt");

                        // In case Process stops while reading data
                        dwarfFortressProcessId = -1;
                        dwarfFortressProcessName = null;
                        dwarfFortressProcessPath = null;
                    }
                }
                else
                {
                    ProcessInfo pi = ProcessInfo.GetDwarfFortressByProcessId(dwarfFortressProcessId);

                    if ((pi == null) || (pi.ProcessName != dwarfFortressProcessName) ||
                        (pi.ExePath != dwarfFortressProcessPath))
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
            List<ProcessInfo> df = ProcessInfo.GetDwarfFortressProcesses(DFName, RelPathGamelog);
            if (df.Count == 0)
            {
                logger.Trace($"No {DFName} running");
                processName = null;
                processPath = null;
                return -1;
            }

            if (df.Count > 1)
            {
                logger.Info($"More than 1 {DFName} is running. Using first PID:{df[0].ProcessID} path:{df[0].ExePath}");
            }

            GameLogPath = df[0].GameLogPath;
            processName = df[0].ProcessName;
            processPath = df[0].ExePath;
            return df[0].ProcessID;
        }


        internal static class interop
        {
            [DllImport("Kernel32.dll")]
            public static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] uint dwFlags, [Out] System.Text.StringBuilder lpExeName, [In, Out] ref uint lpdwSize);
            public static string GetMainModuleFileName(Process process, int buffer = 1024)
            {
                var fileNameBuilder = new System.Text.StringBuilder(buffer);
                uint bufferLength = (uint)fileNameBuilder.Capacity + 1;
                return QueryFullProcessImageName(process.Handle, 0, fileNameBuilder, ref bufferLength) ?
                    fileNameBuilder.ToString() :
                    null;
            }

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
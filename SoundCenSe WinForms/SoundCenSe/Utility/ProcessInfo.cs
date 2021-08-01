using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace SoundCenSe.Utility
{
    public class ProcessInfo
    {
        public readonly string ExePath;
        public readonly string ProcessName;
        public readonly int ProcessID;
        public readonly IntPtr Handle;
        public readonly string GameLogPath;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public ProcessInfo(string exePath, string processName, int processID, IntPtr handle, string gameLogPath)
        {
            ExePath = exePath;
            ProcessName = processName;
            ProcessID = processID;
            Handle = handle;
            GameLogPath = gameLogPath;
        }

        private static string GetExePath(Process p)
        {
            Exception ex1, ex2;

            try
            {
                return p.MainModule.FileName;
            }
            catch (Exception ex)
            {
                ex1 = ex;
            }


            try
            {
                return interop.GetMainModuleFileName(p);
            }
            catch (Exception ex)
            {
                ex2 = ex;
            }

            logger.Error(ex1, $"Failed to find main-module for {p.ProcessName} process using System.Diagnostics PID:{p.Id}");
            logger.Error(ex2, $"Failed to find main-module for {p.ProcessName} process using win32 QueryFullProcessImageName PID:{p.Id}");
            return null;
        }


        /// <summary>
        /// Creates and popilates instance
        /// </summary>
        /// <param name="p"></param>
        /// <param name="RelPathGamelog">If set, checks if gamelog exists at relative path to exe</param>
        /// <returns></returns>
        private static ProcessInfo CreateFrom(Process p, string RelPathGamelog)
        {
            string fileName = GetExePath(p);
            if (fileName == null)
                return null;

            string path = Path.GetDirectoryName(fileName);
            string gamelog = null;

            if (!string.IsNullOrEmpty(RelPathGamelog) )
            {
                gamelog = Path.Combine(path, RelPathGamelog);
                if (!File.Exists(gamelog))
                {
                    logger.Info($"Failed to find gamelog under {path}");
                    return null;
                }
            }

            return new ProcessInfo(fileName, p.ProcessName, p.Id, p.Handle, gamelog);
        }

        public static List<ProcessInfo> GetDwarfFortressProcesses(string DFName, string RelPathGamelog)
        {
            List<ProcessInfo> res = new List<ProcessInfo>();

            List<Process> df = Process.GetProcessesByName(DFName).ToList();
            foreach (Process p in df)
            {
                ProcessInfo pi = CreateFrom(p, RelPathGamelog);
                if ( null == pi )
                    continue;

                res.Add(pi);
            }
            return res;
        }

        public static ProcessInfo GetDwarfFortressByProcessId(int pid)
        {
            try
            {
                Process p = Process.GetProcessById(pid);
                return CreateFrom(p, null);
            }
            catch ( Exception ex )
            {
                logger.Info(ex, $"Failed to create ProcessInfo from PID: {pid}");
                return null;
            }
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

    }
}

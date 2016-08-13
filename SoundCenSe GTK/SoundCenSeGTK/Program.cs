using System;
using Gtk;
using System.IO;
using NLog;

namespace SoundCenSeGTK
{
    class MainClass
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            if (System.IO.Path.DirectorySeparatorChar == '\\')
            {
                if (!VCRedist120())
                {
                    logger.Fatal("Microsoft Visual C++ Redistributable 2013 is not installed.");
                    logger.Fatal("Please download and install from https://download.microsoft.com/download/3/8/7/387A0F10-C0C1-4C74-82A9-4BB741342366/vcredist_x86.exe");
                    return;
                }
                if (!CheckWindowsGtk())
                {
                    logger.Fatal("GTK-Sharp is not installed.");
                    logger.Fatal("Please download and install from http://download.xamarin.com/GTKforWindows/Windows/gtk-sharp-2.12.38.msi");
                    return;
                }
            }
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                NLog.LogManager.GetCurrentClassLogger().Fatal("Uncaught Exception: " + Environment.NewLine + e.ExceptionObject + " " + e.ExceptionObject.GetType().FullName);
            };
            Application.Init();
            MainWindow win = new MainWindow();
            Gtk.Settings settings = Gtk.Settings.Default;
            settings.SetLongProperty("gtk-button-images", 1, "");
            win.Show();
            Application.Run();
        }

        static bool VCRedist120()
        {
            using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\DevDiv\vc\Servicing\12.0\RuntimeMinimum"))
            {
                if (key != null)
                {
                    object val = key.GetValue("Install");
                    if (val != null)
                    {
                        Int32? isInstalled = (Int32?)val;
                        if (isInstalled == 1)
                        {
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        static bool CheckWindowsGtk()
        {
            logger.Warn("Checking dll in registry");
            string location = null;
            Version version = null;
            Version minVersion = new Version(2, 12, 22);
            using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Xamarin\GtkSharp\InstallFolder"))
            {
                if (key != null)
                    location = key.GetValue(null) as string;
            }
            using (var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Xamarin\GtkSharp\Version"))
            {
                if (key != null)
                    Version.TryParse(key.GetValue(null) as string, out version);
            }
            //TODO: check build version of GTK# dlls in GAC
            if (version == null || version < minVersion || location == null || !System.IO.File.Exists(System.IO.Path.Combine(location, "bin", "libgtk-win32-2.0-0.dll")))
            {
                logger.Warn("GTK# is not installed");
                return false;
            }
            var path = System.IO.Path.Combine(location, @"bin");
            try
            {
                if (SetDllDirectory(path))
                {
                    return true;
                }
            }
            catch (EntryPointNotFoundException)
            {
            }
            // this shouldn't happen unless something is weird in Windows
            return false;
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode, SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

    }
}
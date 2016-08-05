using System;
using Gtk;

namespace SoundCenSeGTK
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                NLog.LogManager.GetCurrentClassLogger().Fatal("Uncaught Exception: " + Environment.NewLine + e.ExceptionObject + " " + e.ExceptionObject.GetType().FullName);
            };
			Application.Init ();
			MainWindow win = new MainWindow ();
			Gtk.Settings settings = Gtk.Settings.Default;
			settings.SetLongProperty ("gtk-button-images", 1,"");
			win.Show ();
			Application.Run ();
		}
	}
}

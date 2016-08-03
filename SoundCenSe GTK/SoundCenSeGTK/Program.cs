using System;
using Gtk;

namespace SoundCenSeGTK
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			Gtk.Settings settings = Gtk.Settings.Default;
			settings.SetLongProperty ("gtk-button-images", 1,"");
			win.Show ();
			Application.Run ();
		}
	}
}

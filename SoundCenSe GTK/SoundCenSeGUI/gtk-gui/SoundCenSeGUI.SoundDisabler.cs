
// This file has been generated by the GUI designer. Do not modify.
namespace SoundCenSeGUI
{
	public partial class SoundDisabler
	{
		private global::Gtk.HBox hbox1;
		
		private global::Gtk.Button button1;
		
		private global::Gtk.Label labelSoundName;

		protected virtual void Build ()
		{
			global::Stetic.Gui.Initialize (this);
			// Widget SoundCenSeGUI.SoundDisabler
			global::Stetic.BinContainer.Attach (this);
			this.Name = "SoundCenSeGUI.SoundDisabler";
			// Container child SoundCenSeGUI.SoundDisabler.Gtk.Container+ContainerChild
			this.hbox1 = new global::Gtk.HBox ();
			this.hbox1.Name = "hbox1";
			this.hbox1.Spacing = 6;
			// Container child hbox1.Gtk.Box+BoxChild
			this.button1 = new global::Gtk.Button ();
			this.button1.WidthRequest = 24;
			this.button1.Name = "button1";
			this.button1.UseUnderline = true;
			this.button1.FocusOnClick = false;
			global::Gtk.Image w1 = new global::Gtk.Image ();
			w1.Pixbuf = global::Gdk.Pixbuf.LoadFromResource ("SoundCenSeGUI.DisableSound 15x15.png");
			this.button1.Image = w1;
			this.hbox1.Add (this.button1);
			global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.button1]));
			w2.Position = 0;
			w2.Expand = false;
			w2.Fill = false;
			// Container child hbox1.Gtk.Box+BoxChild
			this.labelSoundName = new global::Gtk.Label ();
			this.labelSoundName.Name = "labelSoundName";
			this.labelSoundName.LabelProp = "";
			this.hbox1.Add (this.labelSoundName);
			global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.hbox1 [this.labelSoundName]));
			w3.Position = 1;
			w3.Expand = false;
			w3.Fill = false;
			this.Add (this.hbox1);
			if ((this.Child != null)) {
				this.Child.ShowAll ();
			}
			this.Show ();
			this.button1.Clicked += new global::System.EventHandler (this.btnDisableClick);
		}
	}
}

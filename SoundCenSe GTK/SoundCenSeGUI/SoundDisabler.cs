using System;
using Misc;

namespace SoundCenSeGUI
{
	[System.ComponentModel.ToolboxItem (true)]
	public partial class SoundDisabler : Gtk.Bin
	{
		private string soundName = "";

		/// <summary>
		/// Gets or sets the name of the sound.
		/// </summary>
		/// <value>The name of the sound.</value>
		public string SoundName {
			get { return soundName; }
			set {
				soundName = value;
                labelSoundName.Text = System.IO.Path.GetFileNameWithoutExtension(value);
			}
		}

		/// <summary>
		/// The sound disabled.
		/// </summary>
		public EventHandler<DisableSoundEventArgs> SoundDisabled;

		/// <summary>
		/// Initializes a new instance of the <see cref="SoundCenSeGUI.SoundDisabler"/> class.
		/// </summary>
		/// <param name="SoundName">Sound name.</param>
		public SoundDisabler (string soundName)
		{
			this.Build ();
            this.SoundName = soundName;
			ShowAll ();
		}

		protected void btnDisableClick (object sender, EventArgs e)
		{
			var handler = SoundDisabled;
			if (handler != null) {
				handler (this, new DisableSoundEventArgs (soundName));
			}
		}
	}
}


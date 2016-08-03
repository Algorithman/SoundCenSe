using System;

namespace Misc
{
	public class DisableSoundEventArgs : EventArgs
	{
		public string Filename {get;set;}
		public DisableSoundEventArgs (string filename)
		{
			this.Filename = filename;
		}
	}
}


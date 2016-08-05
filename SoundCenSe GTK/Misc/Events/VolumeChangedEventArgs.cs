using System;

namespace Misc
{
	public class VolumeChangedEventArgs : EventArgs
	{
		public string Channel { get; set; }

		public double Volume { get; set; }

		public VolumeChangedEventArgs (string channel, double volume)
		{
			Channel = channel;
			Volume = volume;
		}
	}
}


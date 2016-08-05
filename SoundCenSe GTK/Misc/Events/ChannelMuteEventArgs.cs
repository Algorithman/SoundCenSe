using System;

namespace Misc
{
	public class ChannelMuteEventArgs : EventArgs
	{
		public string Channel { get; set; }

		public bool Mute { get; set; }

		public ChannelMuteEventArgs (string channel, bool mute)
		{
			Channel = channel;
            Mute = mute;
		}
	}
}


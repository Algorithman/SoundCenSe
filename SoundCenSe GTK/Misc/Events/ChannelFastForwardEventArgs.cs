using System;

namespace Misc
{
	public class ChannelFastForwardEventArgs : EventArgs
	{public string ChannelName {get;set;}
		
		public ChannelFastForwardEventArgs (string channelName)
		{
			ChannelName = channelName;
		}
	}
}


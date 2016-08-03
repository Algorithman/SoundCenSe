using System;

namespace Misc
{
	public class XMLParseProgressEventArgs : EventArgs
	{
		#region Properties

		public string Filename { get; set; }

		#endregion

		public XMLParseProgressEventArgs(string filename)
		{
			Filename = filename;
		}
	}
}


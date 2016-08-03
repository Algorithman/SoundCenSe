using System;
using System.Collections.Generic;
using System.IO;

namespace Misc
{
	public interface IPlaylistParser
	{
		/// <summary>
		///     Parse the playlist file
		/// </summary>
		/// <param name="parentFilename">Name of the parent XML file</param>
		/// <param name="input">Input stream of the playlist file</param>
		/// <returns>List of audio file names</returns>
		List<string> Parse(string parentFilename, Stream input);
	}
}


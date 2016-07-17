// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: IPlaylistParser.cs
// 
// Last modified: 2016-07-17 22:06

#region Usings

using System.Collections.Generic;
using System.IO;

#endregion

namespace SoundCenSe.Configuration.Sounds.Playlist
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
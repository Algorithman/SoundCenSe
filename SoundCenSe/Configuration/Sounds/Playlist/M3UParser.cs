// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: M3UParser.cs
// 
// Last modified: 2016-07-17 22:06

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using NLog;

#endregion

namespace SoundCenSe.Configuration.Sounds.Playlist
{
    /// <summary>
    ///     Parser for M3U playlists
    /// </summary>
    public class M3UParser : IPlaylistParser
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region IPlaylistParser Members

        /// <summary>
        ///     Parse M3U file stream
        /// </summary>
        /// <param name="parentFilename">Name of the parent XML file</param>
        /// <param name="input">Input stream of the playlist</param>
        /// <returns>List of audio file names</returns>
        public List<string> Parse(string parentFilename, Stream input)
        {
            List<string> playlist = new List<string>();

            TextReader tr = new StreamReader(input);
            string line = tr.ReadLine();

            if (line == "#EXTM3U")
            {
                while (line != null)
                {
                    if (!line.StartsWith("#") && !string.IsNullOrEmpty(line))
                    {
                        string soundFilename = Path.Combine(Path.GetDirectoryName(parentFilename), line);
                        if (!File.Exists(soundFilename))
                        {
                            if (File.Exists(line))
                            {
                                soundFilename = line;
                            }
                            else
                            {
                                logger.Warn("Did not find " + soundFilename + ", ignoring.");
                                soundFilename = null;
                            }
                        }
                        if (!string.IsNullOrEmpty(soundFilename))
                        {
                            playlist.Add(soundFilename);
                        }
                    }
                    line = tr.ReadLine();
                }
            }
            else
            {
                throw new Exception("Wrong header for playlist");
            }

            return playlist;
        }

        #endregion
    }
}
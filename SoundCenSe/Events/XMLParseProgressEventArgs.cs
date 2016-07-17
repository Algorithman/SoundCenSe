// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: XMLParseProgressEventArgs.cs
// 
// Last modified: 2016-07-17 22:06

using System;

namespace SoundCenSe.Events
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
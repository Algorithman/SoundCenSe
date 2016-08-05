// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: XMLParseProgressEventArgs.cs
// 
// Last modified: 2016-07-30 19:37

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
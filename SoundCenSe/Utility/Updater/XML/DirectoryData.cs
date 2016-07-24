// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: DirectoryData.cs
// 
// Last modified: 2016-07-24 13:52

namespace SoundCenSe.Utility.Updater.XML
{
    public class DirectoryData
    {
        #region Properties

        public string Filename { get; set; }

        public string RelativePath { get; set; }
        public string SHA1 { get; set; }
        public long Size { get; set; }

        #endregion
    }
}
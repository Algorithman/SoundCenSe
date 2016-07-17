// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: DirectoryData.cs
// 
// Last modified: 2016-07-17 22:06

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
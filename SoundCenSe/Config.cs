// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: Config.cs
// 
// Last modified: 2016-07-30 19:37

using NLog;
using SoundCenSe.Configuration;

namespace SoundCenSe
{
    public class Config
    {
        #region Fields and Constants

        public static ConfigurationData Instance;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        public Config(string filename)
        {
            Load(filename);
        }

        public static void Load(string filename)
        {
            Instance = ConfigurationData.Load(filename);
        }

        public static void Save(string filename)
        {
            Instance.Save(filename);
        }
    }
}
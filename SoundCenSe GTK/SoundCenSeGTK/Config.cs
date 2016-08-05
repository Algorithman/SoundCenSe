using System;
using NLog;
using Newtonsoft;

namespace SoundCenSeGTK
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


using System;
using NLog;
using System.Xml;

namespace SoundCenSeGTK
{
	public class Attribution
	{
		#region Fields and Constants

		private static readonly Logger logger = LogManager.GetCurrentClassLogger();

		#endregion

		#region Properties

		public string Author { get; set; }
		public string Description { get; set; }
		public string License { get; set; }
		public string Note { get; set; }

		public string Url { get; set; }

		#endregion

		public Attribution(string url, string license, string author, string description, string note)
		{
			Url = url;
			License = license;
			Author = author;
			Description = description;
			Note = note;
		}

		public static Attribution ReadFromXML(XmlNode node, SoundFile soundfile)
		{
			string url = node.ParseStringAttribute("url", null);
			string license = node.ParseStringAttribute("license", null);
			string author = node.ParseStringAttribute("author", null);
			string description = node.ParseStringAttribute("description", null);
			string note = node.ParseStringAttribute("note", "");

			if (string.IsNullOrEmpty(url))
			{
				logger.Info("Attribution url is not set for " + soundfile.Filename);
				return null;
			}
			if (string.IsNullOrEmpty(license))
			{
				logger.Info("Attribution license is not set for " + soundfile.Filename);
				return null;
			}
			if (string.IsNullOrEmpty(author))
			{
				logger.Info("Attribution author is not set for " + soundfile.Filename);
				return null;
			}
			return new Attribution(url, license, author, description, note);
		}

		public override string ToString()
		{
			return Url;
		}
	}
}


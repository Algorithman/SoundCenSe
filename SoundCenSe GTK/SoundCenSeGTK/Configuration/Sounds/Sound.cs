using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Misc;

namespace SoundCenSeGTK
{
	public class Sound : ISound, IDisposable
	{
		#region Fields and Constants

		public List<SoundFile> SoundFiles = new List<SoundFile>();

		#endregion

		#region Properties

		public string AnsiFormat { get; set; }
		public string Channel { get; set; }
		public long Concurrency { get; set; }
		public long Delay { get; set; }
		public bool HaltOnMatch { get; set; }
		public long Hits { get; set; }
		public long LastPlayed { get; set; }
		public string logPattern { get; set; }
		public Loop loop { get; set; }
		public string ParentFile { get; set; }
		public Regex parsedPattern { get; set; }
		public long PlaybackThreshold { get; set; }
		public long Probability { get; set; }

		public int SoundFilesWeightSum { get; set; }
		public long Timeout { get; set; }

		#endregion

		public Sound(string parentFile, List<SoundFile> soundfiles, string pattern, string ansiFormat, Loop _loop,
			string channel, long concurrency, bool haltOnMatch, long timeout, long delay, long probability,
			long playbackThreshold)
		{
			ParentFile = parentFile;
			if (soundfiles != null)
			{
				SoundFiles.AddRange(soundfiles.OrderByDescending(x => x.Weight).ToArray());
			}
			logPattern = pattern;
			loop = _loop;
			Channel = channel;
			Concurrency = concurrency;
			HaltOnMatch = haltOnMatch;
			Hits = 0;
			Probability = probability;
			PlaybackThreshold = playbackThreshold;
			AnsiFormat = ansiFormat;
			SoundFilesWeightSum = 0;
			Delay = delay;
			Timeout = timeout;
			foreach (SoundFile sf in SoundFiles)
			{
				SoundFilesWeightSum += sf.Weight;
			}

			if (!string.IsNullOrEmpty(logPattern))
			{
				parsedPattern = new Regex(logPattern);
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				foreach (SoundFile sf in SoundFiles)
				{
					sf.Dispose();
				}
			}
		}

		public SoundFile GetRandomSoundFile()
		{
			int disabled = SoundFiles.Count(x => x.Disabled);
			if ((SoundFiles.Count > 0) && (disabled < SoundFiles.Count))
			{
				int weightSum = SoundFilesWeightSum;
				if (disabled > 0)
				{
					foreach (SoundFile sf in SoundFiles.Where(x => x.Disabled))
					{
						weightSum -= sf.Weight;
					}
				}
				Random rnd = new Random();
				int weightedrandom = rnd.Next(weightSum);
				foreach (SoundFile sf in SoundFiles.Where(x => !x.Disabled))
				{
					if (sf.Weight > weightedrandom)
					{
						return sf;
					}
					weightedrandom -= sf.Weight;
				}
			}
			return null;
		}

		public bool HasNoSoundFiles()
		{
			return SoundFiles.Count == 0;
		}

		public void Hit()
		{
			Hits++;
		}

		public bool Matches(string line)
		{
			return parsedPattern.IsMatch(line);
		}

		public override string ToString()
		{
			return ParentFile;
		}
	}
}


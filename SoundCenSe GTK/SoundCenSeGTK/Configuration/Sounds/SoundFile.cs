using System;
using System.Collections.Generic;
using Misc;

namespace SoundCenSeGTK
{
	public class SoundFile : ISoundFile, IDisposable
	{
		#region Fields and Constants

		public List<Attribution> Attributions = new List<Attribution>();
		public int Weight = 100;

		#endregion

		#region Properties

		public float BalanceAdjustment { get; set; }

		public bool Disabled { get; set; }

		public string Filename { get; set; }

		public uint Length { get; set; }
		public bool RandomBalance { get; set; }
		public float VolumeAdjustment { get; set; }

		#endregion

		public SoundFile(string filename, int weight, float volumeAdjustment, float balanceAdjustment,
			bool randomBalance)
		{
			Filename = filename;
			Weight = weight;
			VolumeAdjustment = volumeAdjustment;
			BalanceAdjustment = balanceAdjustment;
			RandomBalance = randomBalance;
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
			}
		}

		public override string ToString()
		{
			return Filename;
		}
	}
}


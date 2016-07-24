// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SoundFile.cs
// 
// Last modified: 2016-07-24 13:52

#region Usings

using System;
using System.Collections.Generic;
using NAudio.Wave;
using SoundCenSe.Audio;

#endregion

namespace SoundCenSe.Configuration.Sounds
{
    public class SoundFile : IDisposable
    {
        #region Fields and Constants

        public List<Attribution> Attributions = new List<Attribution>();


        public CachedSound Cache;
        public int Weight = 100;

        #endregion

        #region Properties

        public float BalanceAdjustment { get; set; }

        public bool Disabled { get; set; }

        public string Filename { get; set; }
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
                Cache = null;
            }
        }

        public void GenerateCache(WaveFormat wf)
        {
            if (Cache == null)
            {
                Cache = new CachedSound(Filename, wf);
            }
        }

        public override string ToString()
        {
            return Filename;
        }
    }
}
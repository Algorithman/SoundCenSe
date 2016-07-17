// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: CachedSoundSampleProvider.cs
// 
// Last modified: 2016-07-17 22:06

using System;
using NAudio.Wave;

namespace SoundCenSe.Audio
{
    public class CachedSoundSampleProvider : ISampleProvider
    {
        #region Fields and Constants

        private readonly CachedSound cachedSound;
        private long position;

        #endregion

        public CachedSoundSampleProvider(CachedSound cachedSound)
        {
            this.cachedSound = cachedSound;
        }

        #region ISampleProvider Members

        public int Read(float[] buffer, int offset, int count)
        {
            var availableSamples = cachedSound.AudioData.Length - position;
            var samplesToCopy = Math.Min(availableSamples, count);
            Array.Copy(cachedSound.AudioData, position, buffer, offset, samplesToCopy);
            position += samplesToCopy;
            return (int) samplesToCopy;
        }

        public WaveFormat WaveFormat
        {
            get { return cachedSound.WaveFormat; }
        }

        #endregion
    }
}
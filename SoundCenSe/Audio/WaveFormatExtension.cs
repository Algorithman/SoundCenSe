// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: WaveFormatExtension.cs
// 
// Last modified: 2016-07-17 22:06

#region Usings

using NAudio.Wave;

#endregion

namespace SoundCenSe.Audio
{
    public static class WaveFormatExtension
    {
        public static bool EQUALS(this WaveFormat dme, WaveFormat other)
        {
            bool isEqual = true;
            isEqual &= dme.SampleRate == other.SampleRate;
            isEqual &= dme.Channels == other.Channels;
            isEqual &= dme.BitsPerSample == other.BitsPerSample;
            return isEqual;
        }
    }
}
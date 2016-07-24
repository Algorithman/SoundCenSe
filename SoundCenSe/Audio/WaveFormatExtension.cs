// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: WaveFormatExtension.cs
// 
// Last modified: 2016-07-24 13:52

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
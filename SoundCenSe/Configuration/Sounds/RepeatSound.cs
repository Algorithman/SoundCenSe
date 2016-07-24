// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: RepeatSound.cs
// 
// Last modified: 2016-07-24 13:52

using System.Collections.Generic;
using SoundCenSe.Enums;

namespace SoundCenSe.Configuration.Sounds
{
    public class RepeatSound : Sound
    {
        public RepeatSound(string parentFile, List<SoundFile> soundfiles, string pattern, string ansiFormat, Loop _loop,
            string channel, long concurrency, bool haltOnMatch, long timeout, long delay, long probability,
            long playbackThreshold)
            : base(
                parentFile, soundfiles, pattern, ansiFormat, _loop, channel, concurrency, haltOnMatch, timeout, delay,
                probability, playbackThreshold)
        {
        }
    }
}
// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: DummyPlayerManager.cs
// 
// Last modified: 2016-07-17 22:06

using System.Collections.Generic;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Enums;
using SoundCenSe.Interfaces;

namespace SoundCenSe.Output
{
    public class DummyPlayerManager : IPlayerManager
    {
        #region Fields and Constants

        public readonly Dictionary<string, Sound> Channels = new Dictionary<string, Sound>();

        #endregion

        #region IPlayerManager Members

        public Threshold Threshold { get; set; }
        public float Volume { get; set; }

        public void Play(Sound sound, long x, long y, long z)
        {
            if (!string.IsNullOrEmpty(sound.Channel))
            {
                if (!Channels.ContainsKey(sound.Channel))
                {
                    Channels.Add(sound.Channel, sound);
                }
                else
                {
                    Channels[sound.Channel] = sound;
                }
            }
        }

        #endregion
    }
}
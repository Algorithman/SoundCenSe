using System;
using System.Linq;
using System.Collections.Generic;
using Misc;

namespace SoundCenSeGTK
{
    public class DummyPlayerManager : IPlayerManager
    {
        #region Fields and Constants

        public readonly Dictionary<string, Sound> Channels = new Dictionary<string, Sound>();

        #endregion

        #region IPlayerManager Members

        public Threshold Threshold { get; set; }

        public float Volume { get; set; }

        public void Play(ISound sound, long x, long y, long z)
        {
            if (sound.PlaybackThreshold <= (long)Config.Instance.playbackThreshold)
            {
                if (!string.IsNullOrEmpty(sound.Channel))
                {
                    if (!Channels.ContainsKey(sound.Channel))
                    {
                        Channels.Add(sound.Channel, (Sound)sound);
                    }
                    else
                    {
                        Channels[sound.Channel] = (Sound)sound;
                    }
                }
            }
        }

        #endregion
    }
}


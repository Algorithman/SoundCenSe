// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SFXQueue.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.Collections.Generic;
using SoundCenSe.Configuration.Sounds;

namespace SoundCenSe.Utility
{
    public static class SFXQueue
    {
        #region Fields and Constants

        private static readonly Queue<Tuple<long, long, long, Sound>> soundQueue =
            new Queue<Tuple<long, long, long, Sound>>();

        #endregion

        #region Properties

        public static int Count
        {
            get
            {
                lock (soundQueue)
                {
                    return soundQueue.Count;
                }
            }
        }

        #endregion

        public static void AddSound(Sound s, long x, long y, long z)
        {
            lock (soundQueue)
            {
                soundQueue.Enqueue(new Tuple<long, long, long, Sound>(x, y, z, s));
            }
        }

        public static Tuple<long, long, long, Sound> GetSound()
        {
            lock (soundQueue)
            {
                if (soundQueue.Count == 0)
                {
                    return null;
                }
                return soundQueue.Dequeue();
            }
        }
    }
}
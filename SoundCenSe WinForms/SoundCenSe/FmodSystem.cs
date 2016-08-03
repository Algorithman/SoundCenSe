// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: FmodSystem.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FMOD;

namespace SoundCenSe
{
    public static class FmodSystem
    {
        #region Fields and Constants

        private static FMOD.System _system;

        public static Dictionary<int, Channel> RawChannels = new Dictionary<int, Channel>();

        #endregion

        #region Properties

        public static FMOD.System System
        {
            get
            {
                if (_system == null)
                {
                    Factory.System_Create(out _system);
                    RESULT r = _system.init(32, INITFLAGS.NORMAL, IntPtr.Zero);
                    if (r != RESULT.OK)
                    {
                        MessageBox.Show("Failed: " + r);
                    }
                }
                return _system;
            }
        }

        #endregion

        public static void AddC(Channel c)
        {
            int id;
            c.getIndex(out id);
            if (RawChannels.ContainsKey(id))
            {
                RawChannels[id] = c;
            }
            else
            {
                RawChannels.Add(id, c);
            }
        }
    }
}
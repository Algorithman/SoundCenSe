﻿// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: FmodSystem.cs
// 
// Last modified: 2016-07-30 19:37

using System;
using System.Collections.Generic;
using FMOD;
using NLog;
using System.Diagnostics;

namespace SoundCenSeGTK
{
    public static class FmodSystem
    {
        #region Fields and Constants

        private static Logger logger = LogManager.GetCurrentClassLogger();

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
                        MessageBox.Show("Failed to initialize FMOD sound system: " + r);
                        return null;
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

        public static void ERRCHECK(RESULT result)
        {
            if (result != RESULT.OK)
            {
                logger.Info("FMOD Error: " + result.ToString());
                logger.Info(new StackTrace().ToString());
            }
        }
    }
}
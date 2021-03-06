﻿using System;
using FMOD;
using NLog;
using System.Text.RegularExpressions;
using Misc;
using System.Text;
using System.Threading.Tasks;

namespace SoundCenSeGTK
{
    public class SoundProcessor
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static readonly Logger missingLogger = LogManager.GetLogger("missing");
        internal readonly Regex coordinatePattern;

        internal Sound lastSFX;

        internal readonly Regex repeater = new Regex("^x\\d{1,3}$");
        internal readonly SoundsXML soundsXML;

        #endregion

        public SoundProcessor(SoundsXML sounds)
        {
            soundsXML = sounds;
            coordinatePattern = new Regex("\\[(\\-?\\d),(\\-?\\d),(\\-?\\d)\\] (.*)");
        }

        public virtual void ProcessLine(object sender, GamelogEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                string line = e.Line;

                int matches = 0;
                Sound matchedSound = null;

                if (repeater.IsMatch(line))
                {
                    if (lastSFX != null)
                    {
                        fmodPlayer.Instance.Play(lastSFX, 0, 0, 0);
                    }
                    return;
                }
                Match matcher = coordinatePattern.Match(line);

                long x = long.MinValue;
                long y = long.MinValue;
                long z = long.MinValue;
                if (matcher.Success)
                {
                    x = long.Parse(matcher.Groups[1].Value);
                    y = long.Parse(matcher.Groups[2].Value);
                    z = long.Parse(matcher.Groups[3].Value);
                    line = matcher.Groups[4].Value;
                }
                DateTime dt = DateTime.UtcNow;
                int sndChecks = 0;
                foreach (Sound sound in soundsXML.Sounds)
                {
                    sndChecks++;
                    if (!Config.Instance.disabledSounds.Contains(sound.ParentFile) &&
                        (sound.Matches(line)))
                    {
                        matches++;
                        matchedSound = sound;
                        logger.Info("Message '" + line + "' matched event '" + sound + "' from '" + sound.ParentFile + "'.");

                        fmodPlayer.Instance.Play(sound, x, y, z);

                        if (string.IsNullOrEmpty(sound.Channel))
                        {
                            lastSFX = sound;
                        }

                        if (sound.HaltOnMatch)
                        {
                            logger.Info("Ending Match prematurely as expected.");
                            break;
                        }

                        logger.Info("Continuing for next rule match.");
                    }
                }

                if (matchedSound == null)
                {
                    logger.Info(line);
                    logger.Info("Message '" + line + "' did not match any rule.");
                    missingLogger.Warn(line);
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    if (matchedSound.AnsiFormat != null)
                    {
                        sb.Append(matchedSound.AnsiFormat);
                        sb.Append(line);

                        logger.Info(sb.ToString());
                        logger.Info("Message '" + line + "' matched " + matches + " rules.");
                    }
                }
            });

        }
    }
}


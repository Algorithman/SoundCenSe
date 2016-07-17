// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: SoundProcessor.cs
// 
// Last modified: 2016-07-17 22:06

using System.Text;
using System.Text.RegularExpressions;
using NLog;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Enums;
using SoundCenSe.Events;
using SoundCenSe.Interfaces;

namespace SoundCenSe.Output
{
    public class SoundProcessor
    {
        #region Fields and Constants

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Regex coordinatePattern;

        private Sound lastSFX;
        private readonly IPlayerManager player;

        private readonly Regex repeater = new Regex("^x\\d{1,3}$");
        internal readonly SoundsXML soundsXML;

        private Threshold threshold = Threshold.EVERYTHING;
        private float volume = 1.0f;

        #endregion

        #region Properties

        public Threshold Threshold
        {
            get { return threshold; }
            set
            {
                if (value != threshold)
                {
                    threshold = value;
                    player.Threshold = value;
                }
            }
        }

        public float Volume
        {
            get { return volume; }
            set
            {
                if (value != volume)
                {
                    volume = value;
                    player.Volume = value;
                }
            }
        }

        #endregion

        public SoundProcessor(SoundsXML sounds, IPlayerManager playerManager)
        {
            player = playerManager;
            soundsXML = sounds;
            coordinatePattern = new Regex("\\[(\\-?\\d),(\\-?\\d),(\\-?\\d)\\] (.*)");
        }

        public virtual void ProcessLine(object sender, GamelogEventArgs e)
        {
            string line = e.Line;

            int matches = 0;
            Sound matchedSound = null;

            if (repeater.IsMatch(line))
            {
                if (lastSFX != null)
                {
                    player.Play(lastSFX, 0, 0, 0);
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

            foreach (Sound sound in soundsXML.Sounds)
            {
                if (!Config.Instance.disabledSounds.Contains(sound.ParentFile) &&
                    (sound.Matches(line)))
                {
                    matches++;
                    matchedSound = sound;
                    logger.Info("Message '" + line + "' matched event '" + sound + "' from '" + sound.ParentFile + "'.");

                    player.Play(sound, x, y, z);

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
        }
    }
}
// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: DummySoundProcessor.cs
// 
// Last modified: 2016-07-30 19:37

using System.Linq;
using System.Text.RegularExpressions;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Events;

namespace SoundCenSe.Output
{
    public class DummySoundProcessor : SoundProcessor
    {
        #region Properties

        public DummyPlayerManager DummyPlayerManager { get; set; }

        #endregion

        public DummySoundProcessor(SoundsXML sounds) : base(sounds)
        {
            this.soundsXML.Sounds = sounds.Sounds.Where(x => !string.IsNullOrEmpty(x.Channel)).ToList();
        }

        public override void ProcessLine(object sender, GamelogEventArgs e)
        {
            string line = e.Line;

            int matches = 0;
            Sound matchedSound = null;

            if (this.repeater.IsMatch(line))
            {
                if (this.lastSFX != null)
                {
                    DummyPlayerManager.Play(this.lastSFX, 0, 0, 0);
                }
                return;
            }
            Match matcher = this.coordinatePattern.Match(line);

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

            foreach (Sound sound in this.soundsXML.Sounds)
            {
                if (!Config.Instance.disabledSounds.Contains(sound.ParentFile) &&
                    (sound.Matches(line)))
                {
                    matches++;
                    matchedSound = sound;

                    DummyPlayerManager.Play(sound, x, y, z);

                    if (string.IsNullOrEmpty(sound.Channel))
                    {
                        this.lastSFX = sound;
                    }

                    if (sound.HaltOnMatch)
                    {
                        break;
                    }
                }
            }
        }
    }
}
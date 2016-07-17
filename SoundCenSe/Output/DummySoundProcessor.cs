// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundSenseCS
// Project: SoundSenseCS
// File: DummySoundProcessor.cs
// 
// Last modified: 2016-07-17 22:06

using System.Linq;
using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Interfaces;

namespace SoundCenSe.Output
{
    public class DummySoundProcessor : SoundProcessor
    {
        public DummySoundProcessor(SoundsXML sounds, IPlayerManager playerManager) : base(sounds, playerManager)
        {
            this.soundsXML.Sounds = sounds.Sounds.Where(x => !string.IsNullOrEmpty(x.Channel)).ToList();
        }
    }
}
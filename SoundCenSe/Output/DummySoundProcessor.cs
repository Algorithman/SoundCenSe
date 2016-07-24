// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: DummySoundProcessor.cs
// 
// Last modified: 2016-07-24 13:52

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
// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: SoundSoundFile.cs
// 
// Last modified: 2016-07-30 19:37

using SoundCenSe.Configuration.Sounds;

namespace SoundCenSe.fmodInternal
{
    public class SoundSoundFile
    {
        #region Properties

        public FMOD.Sound fmodSound { get; set; }
        public Sound Sound { get; set; }
        public SoundFile SoundFile { get; set; }

        #endregion

        public SoundSoundFile(Sound sound, SoundFile soundFile)
        {
            Sound = sound;
            SoundFile = soundFile;
        }
    }
}
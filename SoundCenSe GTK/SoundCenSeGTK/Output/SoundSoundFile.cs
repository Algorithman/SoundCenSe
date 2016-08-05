using System;
using Misc;

namespace SoundCenSeGTK
{
    public class SoundSoundFile : ISoundSoundFile
    {
        #region Properties

        public FMOD.Sound fmodSound { get; set; }
        public ISound Sound { get; set; }
        public ISoundFile SoundFile { get; set; }

        #endregion

        public SoundSoundFile(Sound sound, SoundFile soundFile)
        {
            Sound = sound;
            SoundFile = soundFile;
        }
    }
}


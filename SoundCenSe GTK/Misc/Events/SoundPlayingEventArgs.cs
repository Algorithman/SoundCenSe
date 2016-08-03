using System;

namespace Misc
{
    public class SoundPlayingEventArgs : EventArgs
    {
        #region Properties

        public bool Mute { get; set; }

        public ISound Sound { get; set; }
        public ISoundFile SoundFile { get; set; }
        public float Volume { get; set; }

        #endregion

        public SoundPlayingEventArgs(ISound sound, ISoundFile soundFile, bool mute, float volume)
        {
            Sound = sound;
            SoundFile = soundFile;
            Mute = mute;
            Volume = volume;
        }
    }
}


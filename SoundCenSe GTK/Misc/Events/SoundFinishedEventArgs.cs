using System;

namespace Misc
{
    public class SoundFinishedEventArgs : EventArgs
    {
        #region Properties

        public ISound Sound { get; set; }
        public ISoundFile SoundFile { get; set; }

        #endregion

        public SoundFinishedEventArgs(ISound sound, ISoundFile soundFile)
        {
            Sound = sound;
            SoundFile = soundFile;
        }
    }
}


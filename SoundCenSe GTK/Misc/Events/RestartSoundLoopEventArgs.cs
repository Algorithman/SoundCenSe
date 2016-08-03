using System;

namespace Misc
{
    public class RestartSoundLoopEventArgs : EventArgs
    {
        #region Properties

        public ISoundSoundFile SoundSoundFile { get; set; }

        #endregion

        public RestartSoundLoopEventArgs(ISoundSoundFile soundSoundFile)
        {
            SoundSoundFile = soundSoundFile;
        }
    }
}


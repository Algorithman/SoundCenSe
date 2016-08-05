using System;

namespace Misc
{
    public interface ISoundSoundFile
    {
        ISound Sound { get; set; }
        ISoundFile SoundFile { get; set; }
    }
}


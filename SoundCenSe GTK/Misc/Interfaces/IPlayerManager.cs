using System;

namespace Misc
{
    public interface IPlayerManager
    {
        #region Properties

        Threshold Threshold { get; set; }
        float Volume { get; set; }

        #endregion

        void Play(ISound sound, long x, long y, long z);

    }
}


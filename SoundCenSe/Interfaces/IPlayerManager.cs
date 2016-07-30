// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: IPlayerManager.cs
// 
// Last modified: 2016-07-30 19:37

using SoundCenSe.Configuration.Sounds;
using SoundCenSe.Enums;

namespace SoundCenSe.Interfaces
{
    public interface IPlayerManager
    {
        #region Properties

        Threshold Threshold { get; set; }
        float Volume { get; set; }

        #endregion

        void Play(Sound sound, long x, long y, long z);
    }
}
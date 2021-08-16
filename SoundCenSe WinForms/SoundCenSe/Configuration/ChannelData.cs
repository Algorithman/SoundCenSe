// 
// SoundSense C# Port aka SoundCenSe
// 
// Solution: SoundCenSe
// Project: SoundCenSe
// File: ChannelData.cs
// 
// Last modified: 2016-07-30 19:37

namespace SoundCenSe.Configuration
{
    public class ChannelData
    {
        #region Properties

        public string Channel { get; set; }
        public bool Mute { get; set; }
        public float Volume { get; set; }

        #endregion

        public override string ToString()
        {
            string vol = Mute ? "muted" : Volume.ToString();
            return $"Channel {Channel} {vol}";
        }
    }
}
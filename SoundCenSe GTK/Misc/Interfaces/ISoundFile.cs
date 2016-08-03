using System;

namespace Misc
{
    public interface ISoundFile
    {
         float BalanceAdjustment { get; set; }

         bool Disabled { get; set; }

         string Filename { get; set; }

         uint Length { get; set; }
         bool RandomBalance { get; set; }
         float VolumeAdjustment { get; set; }

    }
}


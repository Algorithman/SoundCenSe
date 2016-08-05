using System;
using System.Text.RegularExpressions;

namespace Misc
{
    public interface ISound
    {
        string AnsiFormat { get; set; }

        string Channel { get; set; }

        long Concurrency { get; set; }

        long Delay { get; set; }

        bool HaltOnMatch { get; set; }

        long Hits { get; set; }

        long LastPlayed { get; set; }

        string logPattern { get; set; }

        Loop loop { get; set; }

        string ParentFile { get; set; }

        Regex parsedPattern { get; set; }

        long PlaybackThreshold { get; set; }

        long Probability { get; set; }

        int SoundFilesWeightSum { get; set; }

        long Timeout { get; set; }
    }
}


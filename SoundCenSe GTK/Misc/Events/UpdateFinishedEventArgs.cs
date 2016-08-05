using System;

namespace Misc
{
    public class UpdateFinishedEventArgs:EventArgs
    {
        public bool ReloadNeeded { get; set; }

        public UpdateFinishedEventArgs(bool reload)
        {
            ReloadNeeded = reload;
        }
    }
}


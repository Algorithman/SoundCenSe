using System;

namespace Misc
{
    public class GamelogEventArgs : EventArgs
    {
        #region Properties

        public string Line { get; set; }

        #endregion

        public GamelogEventArgs(string line)
        {
            Line = line;
        }
    }
}


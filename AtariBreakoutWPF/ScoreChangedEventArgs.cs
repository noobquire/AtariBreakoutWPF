using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtariBreakoutWPF
{
    class ScoreChangedEventArgs : EventArgs
    {
        public int NewScore { get; }

        public ScoreChangedEventArgs(int newScore)
        {
            NewScore = newScore;
        }
    }
}

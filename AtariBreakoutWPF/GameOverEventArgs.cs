using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtariBreakoutWPF
{
    class GameOverEventArgs : EventArgs
    {
        public int FinalScore { get; }

        public GameOverEventArgs(int finalScore)
        {
            FinalScore = finalScore;
        }
    }
}

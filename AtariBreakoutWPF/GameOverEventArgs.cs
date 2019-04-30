using System;

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

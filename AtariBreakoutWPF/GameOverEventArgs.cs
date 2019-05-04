using System;

namespace AtariBreakoutWPF
{
    public sealed class GameOverEventArgs : EventArgs
    {
        public GameOverEventArgs(int finalScore)
        {
            FinalScore = finalScore;
        }

        public int FinalScore { get; }
    }
}
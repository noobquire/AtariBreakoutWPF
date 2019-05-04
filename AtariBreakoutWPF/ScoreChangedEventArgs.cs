using System;

namespace AtariBreakoutWPF
{
    public sealed class ScoreChangedEventArgs : EventArgs
    {
        public ScoreChangedEventArgs(int newScore)
        {
            NewScore = newScore;
        }

        public int NewScore { get; }
    }
}
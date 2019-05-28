using System;

namespace AtariBreakoutWPF
{
    public sealed class GameOverEventArgs : EventArgs
    {
        public GameOverEventArgs(int finalScore, GameOverReason reason)
        {
            FinalScore = finalScore;
            Reason = reason;
        }

        public int FinalScore { get; }
        public GameOverReason Reason { get;  }
    }

    public enum GameOverReason
    {
        Won,
        Lost,
        Restart,
        Closing
    }
}
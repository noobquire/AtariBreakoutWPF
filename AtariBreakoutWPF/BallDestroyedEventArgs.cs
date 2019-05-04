using System;

namespace AtariBreakoutWPF
{
    public sealed class BallDestroyedEventArgs : EventArgs
    {
        public BallDestroyedEventArgs(int newBallCount)
        {
            NewBallCount = newBallCount;
        }

        public int NewBallCount { get; }
    }
}
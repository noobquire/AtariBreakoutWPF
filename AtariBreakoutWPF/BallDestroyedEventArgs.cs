using System;

namespace AtariBreakoutWPF
{
    class BallDestroyedEventArgs : EventArgs    
    {
        public int NewBallCount { get; }

        public BallDestroyedEventArgs(int newBallCount)
        {
            NewBallCount = newBallCount;
        }
    }
}

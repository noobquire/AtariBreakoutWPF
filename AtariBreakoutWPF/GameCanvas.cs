using System.Collections.Generic;
using System.Windows.Controls;

namespace AtariBreakoutWPF
{
    public class GameCanvas
    {
        public readonly Canvas Canvas;
        public readonly IList<Brick> Bricks = new List<Brick>();
        public double Height => Canvas.Height;
        public double Width => Canvas.Width;
        public int BallCount;
        public Paddle Paddle;
        public BouncyBall Ball; // TODO: set back to private

        public GameCanvas(Canvas canvas)
        {
            Canvas = canvas;
        }
    }
}
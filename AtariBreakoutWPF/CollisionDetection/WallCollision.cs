using System.Windows;

namespace AtariBreakoutWPF.CollisionDetection
{
    public class WallCollision
    {
        private readonly GameCanvas _gameCanvas;

        public WallCollision(GameCanvas gameCanvas)
        {
            _gameCanvas = gameCanvas;
        }

        public static implicit operator bool(WallCollision wallCollision)
        {
            return true;
        }

        public bool Top(Point newPosition)
        {
            return newPosition.Y <= 0;
        }

        public bool Bottom(Point newPosition)
        {
            return newPosition.Y >= _gameCanvas.Height - _gameCanvas.Ball.Shape.Height;
        }

        public bool Left(Point newPosition)
        {
            return newPosition.X <= 0;
        }

        public bool Right(Point newPosition)
        {
            return newPosition.X >= _gameCanvas.Width - _gameCanvas.Ball.Shape.Width;
        }

        public bool Vertical(Point newPosition)
        {
            return Left(newPosition) || Right(newPosition);
        }

        public bool Horizontal(Point newPosition)
        {
            return Top(newPosition) | Bottom(newPosition);
        }
    }
}
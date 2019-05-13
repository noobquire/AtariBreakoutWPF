using System.Windows;

namespace AtariBreakoutWPF.CollisionDetection
{
    public class PaddleCollision
    {
        private readonly GameCanvas _gameCanvas;

        public PaddleCollision(GameCanvas gameCanvas)
        {
            _gameCanvas = gameCanvas;
        }

        public bool Horizontal(Point ballPosition)
        {
            return Vertical(ballPosition, _gameCanvas.Paddle.Position) &&
                   !Horizontal(ballPosition, _gameCanvas.Paddle.Position);
        }

        public bool Vertical(Point ballPosition)
        {
            return Horizontal(ballPosition, _gameCanvas.Paddle.Position) &&
                   !Vertical(ballPosition, _gameCanvas.Paddle.Position);
        }

        public bool Vertical(Point ballPosition, Point paddlePosition)
        {
            return ballPosition.X >= paddlePosition.X - _gameCanvas.Ball.Shape.Width &&
                   ballPosition.X <= paddlePosition.X + _gameCanvas.Paddle.Width;
        }

        public bool Horizontal(Point ballPosition, Point paddlePosition)
        {
            return ballPosition.Y >= paddlePosition.Y - _gameCanvas.Ball.Shape.Height &&
                   ballPosition.Y <= paddlePosition.Y + _gameCanvas.Paddle.Height;
        }

        public bool VerticalAndHorizontal(Point ballPosition, Point paddlePosition)
        {
            return Horizontal(ballPosition, paddlePosition) && Vertical(ballPosition, paddlePosition);
        }
    }
}
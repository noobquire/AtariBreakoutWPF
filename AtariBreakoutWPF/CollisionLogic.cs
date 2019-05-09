using System.Windows;

namespace AtariBreakoutWPF
{
    public class CollisionLogic
    {
        private GameCanvas _gameCanvas;

        public CollisionLogic(GameCanvas gameCanvas)
        {
            _gameCanvas = gameCanvas;
        }
        public bool CollidesWithBrickVertical(Point newPosition, Brick brick)
        {
            return newPosition.X >= brick.Position.X - _gameCanvas.Ball.Shape.Width &&
                   newPosition.X <= brick.Position.X + Brick.Width;
        }
        public bool CollidesWithBrickHorizontal(Point newPosition, Brick brick)
        {
            return newPosition.Y >= brick.Position.Y - _gameCanvas.Ball.Shape.Width &&
                   newPosition.Y <= brick.Position.Y + Brick.Height;
        }
        public bool BallCollidesWithPaddleHorizontally(Point oldPosition)
        {
            return CollidesWithPaddleVertical(oldPosition, _gameCanvas.Paddle.Position) &&
                   !CollidesWithPaddleHorizontal(oldPosition, _gameCanvas.Paddle.Position);
        }
        public bool BallCollidesWithPaddleVertically(Point oldPosition)
        {
            return CollidesWithPaddleHorizontal(oldPosition, _gameCanvas.Paddle.Position) &&
                   !CollidesWithPaddleVertical(oldPosition, _gameCanvas.Paddle.Position);
        }
        public bool BallCollidesWithPaddle(Point newPosition)
        {
            return CollidesWithPaddleHorizontal(newPosition, _gameCanvas.Paddle.Position) &&
                   CollidesWithPaddleVertical(newPosition, _gameCanvas.Paddle.Position);
        }
        public bool CollidesWithPaddleVertical(Point ballPosition, Point paddlePosition)
        {
            return ballPosition.X >= paddlePosition.X - _gameCanvas.Ball.Shape.Width &&
                   ballPosition.X <= paddlePosition.X + _gameCanvas.Paddle.Width;
        }
        public bool CollidesWithPaddleHorizontal(Point ballPosition, Point paddlePosition)
        {
            return ballPosition.Y >= paddlePosition.Y - _gameCanvas.Ball.Shape.Height &&
                   ballPosition.Y <= paddlePosition.Y + _gameCanvas.Paddle.Height;
        }

    }
}
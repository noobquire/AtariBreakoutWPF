using System;
using System.Windows;
using System.Windows.Media;
using AtariBreakoutWPF.CollisionDetection;
using static AtariBreakoutWPF.Utility;

namespace AtariBreakoutWPF
{
    public class MovementLogic
    {
        private GameCanvas _gameCanvas;
        private BallCollision _ballCollision;

        public MovementLogic(GameCanvas gameCanvas)
        {
            _gameCanvas = gameCanvas;
            _ballCollision = new BallCollision(_gameCanvas);
        }
        public void MoveBall()
        {
            var oldPosition = _gameCanvas.Ball.Position;
            var newPosition = new Point(oldPosition.X + _gameCanvas.Ball.MoveVector.X * _gameCanvas.Ball.Speed,
                oldPosition.Y + _gameCanvas.Ball.MoveVector.Y * _gameCanvas.Ball.Speed);

            
            CheckPaddleCollision(newPosition, oldPosition);
            CheckBrickCollision(newPosition, oldPosition);
            CheckWallCollision(newPosition); // ball collision has to be checked last because ball may get destroyed
        }
        public void MovePaddle(Direction direction)
        {
            if (direction == Direction.Default) return;
            var oldPosition = _gameCanvas.Paddle.Position;
            var ballPosition = _gameCanvas.Ball.Position;
            double newXCoordinate = direction == Direction.Left ? oldPosition.X - 3 : oldPosition.X + 3;
            var newPaddlePosition = new Point(newXCoordinate, oldPosition.Y);

            if (_ballCollision.WithPaddle.Horizontal(ballPosition, newPaddlePosition)
                    && _ballCollision.WithPaddle.Vertical(ballPosition, newPaddlePosition)) return;
            if (newPaddlePosition.X < _gameCanvas.Width - _gameCanvas.Paddle.Width && newPaddlePosition.X > 0)
                SetPosition(_gameCanvas.Paddle.Shape, newPaddlePosition);
        }

        private void CheckBrickCollision(Point newPosition, Point oldPosition)
        {
            for (var i = 0; i < _gameCanvas.Bricks.Count; i++)
            {
                var brick = _gameCanvas.Bricks[i];
                if (_ballCollision.WithBrick.VerticalAndHorizontal(newPosition, brick))
                {
                    if (_ballCollision.WithBrick.OnlyHorizontal(oldPosition, brick))
                        _gameCanvas.Ball.Bounce(Direction.Vertical);

                    else if (_ballCollision.WithBrick.OnlyVertical(oldPosition, brick))
                        _gameCanvas.Ball.Bounce(Direction.Horizontal);

                    _gameCanvas.DestroyBrick(brick);
                }
            }
        }
        private void CheckPaddleCollision(Point newPosition, Point oldPosition)
        {
            var centerOfPaddleX = _gameCanvas.Paddle.Position.X + _gameCanvas.Paddle.Width / 2;
            var paddlePosition = _gameCanvas.Paddle.Position;
            if (_ballCollision.WithPaddle.VerticalAndHorizontal(newPosition, paddlePosition))
            {
                double distanceFromCenterOfPaddle;
                if (_ballCollision.WithPaddle.Vertical(oldPosition))
                {
                    distanceFromCenterOfPaddle = _gameCanvas.Paddle.Width / 2;

                    _gameCanvas.Ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Vertical, _gameCanvas.Paddle.Width);
                }
                else if (_ballCollision.WithPaddle.Horizontal(oldPosition, paddlePosition))
                {
                    var centerOfBallX = oldPosition.X + _gameCanvas.Ball.Shape.Width / 2;
                    distanceFromCenterOfPaddle = Math.Abs(centerOfPaddleX - centerOfBallX);
                    if (distanceFromCenterOfPaddle > _gameCanvas.Paddle.Width / 2) distanceFromCenterOfPaddle = _gameCanvas.Paddle.Width / 2;

                    _gameCanvas.Ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Horizontal, _gameCanvas.Paddle.Width);
                }
                _gameCanvas.Paddle.Shape.Fill = Brushes.GreenYellow;
            }
            else
            {
                _gameCanvas.Paddle.Shape.Fill = Brushes.DimGray;
            }
        }
        private void CheckWallCollision(Point newPosition)
        {

            if (_ballCollision.OutBounds.Top(newPosition))
            {
                _gameCanvas.Ball.Bounce(Direction.Horizontal);
            }
            else if (_ballCollision.OutBounds.Vertical(newPosition))
            {
                _gameCanvas.Ball.Bounce(Direction.Vertical);
            }
            else if (_ballCollision.OutBounds.Bottom(newPosition))
            {
                _gameCanvas.DestroyBall();
                _gameCanvas.AddBall();
            }
            else
            {
                SetPosition(_gameCanvas.Ball.Shape, newPosition);
            }
        }

    }
}
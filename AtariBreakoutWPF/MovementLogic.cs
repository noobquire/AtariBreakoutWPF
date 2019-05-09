using System;
using System.Windows;
using System.Windows.Media;
using static AtariBreakoutWPF.Utility;

namespace AtariBreakoutWPF
{
    public class MovementLogic
    {
        private CollisionLogic _ballCollision;
        private GameCanvas _gameCanvas;
        private GameCanvasBuilder _builder; // todo add destroyer/controller/etc, build should not be used here

        public MovementLogic(GameCanvas gameCanvas)
        {
            _ballCollision = new CollisionLogic(gameCanvas);
            _builder = new GameCanvasBuilder(gameCanvas.Canvas);
            _gameCanvas = gameCanvas;
        }
                public void MoveBall()
        {
            var oldPosition = _gameCanvas.Ball.Position;
            var newPosition = new Point(oldPosition.X + _gameCanvas.Ball.MoveVector.X * _gameCanvas.Ball.Speed,
                oldPosition.Y + _gameCanvas.Ball.MoveVector.Y * _gameCanvas.Ball.Speed);

            CheckWallCollision(newPosition);
            CheckPaddleCollision(newPosition, oldPosition);
            CheckBrickCollision(newPosition, oldPosition);
        }
        public void MovePaddle(Direction direction)
        {
        
            var oldPosition = _gameCanvas.Paddle.Position;
            if (direction == Direction.Left)
            {
                var newPositon = new Point(oldPosition.X - 3,
                    oldPosition.Y);
                if (_ballCollision.CollidesWithPaddleHorizontal(_gameCanvas.Ball.Position, newPositon)
                    && _ballCollision.CollidesWithPaddleVertical(_gameCanvas.Ball.Position, newPositon)) return;
                if (newPositon.X < _gameCanvas.Width - _gameCanvas.Paddle.Width && newPositon.X > 0)
                    SetPosition(_gameCanvas.Paddle.Shape, newPositon);
            }
            else if (direction == Direction.Right)
            {
                var newPositon = new Point(oldPosition.X + 3,
                    oldPosition.Y);
                if (_ballCollision.CollidesWithPaddleHorizontal(_gameCanvas.Ball.Position, newPositon)
                    && _ballCollision.CollidesWithPaddleVertical(_gameCanvas.Ball.Position, newPositon)) return;
                if (newPositon.X < _gameCanvas.Width - _gameCanvas.Paddle.Width && newPositon.X > 0)
                    SetPosition(_gameCanvas.Paddle.Shape, newPositon);
            }
        }

        private void CheckBrickCollision(Point newPosition, Point oldPosition)
        {
            for (var i = 0; i < _gameCanvas.Bricks.Count; i++)
            {
                var brick = _gameCanvas.Bricks[i];
                if (_ballCollision.CollidesWithBrickHorizontal(newPosition, brick) 
                    && _ballCollision.CollidesWithBrickVertical(newPosition, brick))
                {
                    if (_ballCollision.CollidesWithBrickHorizontal(oldPosition, brick) &&
                        !_ballCollision.CollidesWithBrickVertical(oldPosition, brick))
                        _gameCanvas.Ball.Bounce(Direction.Vertical);
                    else if (_ballCollision.CollidesWithBrickVertical(oldPosition, brick) &&
                             !_ballCollision.CollidesWithBrickHorizontal(oldPosition, brick))
                        _gameCanvas.Ball.Bounce(Direction.Horizontal);

                    _gameCanvas.DestroyBrick(brick);
                }
            }
        }
        private void CheckPaddleCollision(Point newPosition, Point oldPosition)
        {
            var centerOfPaddleX = _gameCanvas.Paddle.Position.X + _gameCanvas.Paddle.Width / 2;
            double distanceFromCenterOfPaddle;

            if (_ballCollision.BallCollidesWithPaddleVertically(oldPosition))
            {
                distanceFromCenterOfPaddle = _gameCanvas.Paddle.Width / 2;

                _gameCanvas.Ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Vertical, _gameCanvas.Paddle.Width);
            }
            else if (_ballCollision.BallCollidesWithPaddleHorizontally(oldPosition))
            {
                var centerOfBallX = _gameCanvas.Ball.Position.X + _gameCanvas.Ball.Shape.Width / 2;
                distanceFromCenterOfPaddle = Math.Abs(centerOfPaddleX - centerOfBallX);
                if (distanceFromCenterOfPaddle > _gameCanvas.Paddle.Width / 2) distanceFromCenterOfPaddle = _gameCanvas.Paddle.Width / 2;

                _gameCanvas.Ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Horizontal, _gameCanvas.Paddle.Width);
            }
            
            if (_ballCollision.BallCollidesWithPaddle(newPosition))
            {
                _gameCanvas.Paddle.Shape.Fill = Brushes.GreenYellow;
            }
            else
            {
                _gameCanvas.Paddle.Shape.Fill = Brushes.DimGray;
            }
        }
        private void CheckWallCollision(Point newPosition)
        {
            var outBoundsVertical = newPosition.X <= 0 || newPosition.X >= _gameCanvas.Width - _gameCanvas.Ball.Shape.Width;
            var outBoundsTop = newPosition.Y <= 0;
            var outBoundsBottom = newPosition.Y >= _gameCanvas.Height - _gameCanvas.Ball.Shape.Height;

            if (outBoundsTop)
            {
                _gameCanvas.Ball.Bounce(Direction.Horizontal);
            }
            else if (outBoundsVertical)
            {
                _gameCanvas.Ball.Bounce(Direction.Vertical);
            }
            else if (outBoundsBottom)
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
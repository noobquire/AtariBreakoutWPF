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
            Point oldPosition;
            Point newPosition = new Point();
            for (int i = 0; i < _gameCanvas.Ball.Speed; i++)
            {
                oldPosition = _gameCanvas.Ball.Position;
                newPosition = new Point(oldPosition.X + _gameCanvas.Ball.MoveVector.X,
                    oldPosition.Y + _gameCanvas.Ball.MoveVector.Y);
                double distanceFromCenterOfPaddle;
                Direction bounceDirection;
                if (PaddleCollision(newPosition, oldPosition, out distanceFromCenterOfPaddle, out bounceDirection))
                {
                    _gameCanvas.Ball.BounceOffPaddle(distanceFromCenterOfPaddle, bounceDirection, _gameCanvas.Paddle.Width);
                    _gameCanvas.Paddle.Shape.Fill = Brushes.GreenYellow;
                    return;
                }
                _gameCanvas.Paddle.Shape.Fill = Brushes.DimGray;

                if (BrickCollision(newPosition, oldPosition, out bounceDirection))
                {
                    _gameCanvas.Ball.Bounce(bounceDirection);
                    return;
                }

                if (WallCollision(newPosition, out bounceDirection))
                {
                    if (bounceDirection == Direction.OutOfBounds)
                    {
                        _gameCanvas.DestroyBall();
                        _gameCanvas.AddBall();
                        return;
                    }
                    _gameCanvas.Ball.Bounce(bounceDirection);
                    return;
                }
                SetPosition(_gameCanvas.Ball.Shape, newPosition);
            }
            

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

        private bool BrickCollision(Point newPosition, Point oldPosition, out Direction bounceDirection)
        {
            bounceDirection = Direction.Default;
            for (var i = 0; i < _gameCanvas.Bricks.Count; i++)
            {
                var brick = _gameCanvas.Bricks[i];

                if (_ballCollision.WithBrick.VerticalAndHorizontal(newPosition, brick))
                {
                    if (_ballCollision.WithBrick.OnlyHorizontal(oldPosition, brick))
                    {
                        bounceDirection = Direction.Vertical;
                    }
                    else if (_ballCollision.WithBrick.OnlyVertical(oldPosition, brick))
                    {
                        bounceDirection = Direction.Horizontal;
                    }
                    _gameCanvas.DestroyBrick(brick);
                    return true;
                }
            }
            return false;
        }
        private bool PaddleCollision(Point newPosition, Point oldPosition, out double distanceFromCenterOfPaddle, out Direction bounceDirection)
        {
            var centerOfPaddleX = _gameCanvas.Paddle.Position.X + _gameCanvas.Paddle.Width / 2;
            var paddlePosition = _gameCanvas.Paddle.Position;
            distanceFromCenterOfPaddle = 0;
            bounceDirection = Direction.Default;
            if (_ballCollision.WithPaddle.VerticalAndHorizontal(newPosition, paddlePosition))
            {
                if (_ballCollision.WithPaddle.Vertical(oldPosition))
                {
                    distanceFromCenterOfPaddle = _gameCanvas.Paddle.Width / 2;
                    bounceDirection = Direction.Vertical;
                    return true;
                }

                if (_ballCollision.WithPaddle.Horizontal(oldPosition, paddlePosition))
                {
                    var centerOfBallX = oldPosition.X + _gameCanvas.Ball.Shape.Width / 2;
                    distanceFromCenterOfPaddle = Math.Abs(centerOfPaddleX - centerOfBallX);
                    if (distanceFromCenterOfPaddle > _gameCanvas.Paddle.Width / 2) distanceFromCenterOfPaddle = _gameCanvas.Paddle.Width / 2;
                    bounceDirection = Direction.Horizontal;
                    return true;
                }

            }
            return false;
        }
        private bool WallCollision(Point newPosition, out Direction bounceDirection)
        {
            bounceDirection = Direction.Default;

            if (_ballCollision.OutBounds.Top(newPosition))
            {
                bounceDirection = Direction.Horizontal;
                return true;
            }

            if (_ballCollision.OutBounds.Vertical(newPosition))
            {
                bounceDirection = Direction.Vertical;
                return true;
            }
            if (_ballCollision.OutBounds.Bottom(newPosition))
            {
                bounceDirection = Direction.OutOfBounds;
                return true;
            }
            return false;
        }

    }
}
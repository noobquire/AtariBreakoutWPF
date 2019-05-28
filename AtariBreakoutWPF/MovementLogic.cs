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
        
        /// <summary>
        /// Moves the ball for minimal distance on the game field for every point of its speed.
        /// Also makes required checks to detect collisions between ball and paddle, bricks or walls.
        /// If such collision is detected, the ball bounces accordingly.
        /// </summary>
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
        
        /// <summary>
        /// Checks required yo determine if the ball has to accelerate.
        /// If it has to, it's speed is increased by constant amount.
        /// Ball acceleration conditions are connected to hitting the bricks.
        /// </summary>
        /// <param name="brick">Bricks which is hit.</param>
        private void CheckBallAccelerationConditions(Brick brick)
        {
            if (!_gameCanvas.Ball.RedBrickHit && brick.Color == Brick.BrickColor.Red)
            {
                _gameCanvas.Ball.RedBrickHit = true;
                _gameCanvas.Ball.Speed += BouncyBall.Acceleration;
            }

            if (!_gameCanvas.Ball.OrangeBrickHit && brick.Color == Brick.BrickColor.Orange)
            {
                _gameCanvas.Ball.OrangeBrickHit = true;
                _gameCanvas.Ball.Speed += BouncyBall.Acceleration;
            }
            if (_gameCanvas.Ball.HitCount == 4 || _gameCanvas.Ball.HitCount == 12) _gameCanvas.Ball.Speed += BouncyBall.Acceleration;
        }
        
        /// <summary>
        /// Moves the paddle fro minimal distance on the game field.
        /// Also checks if the new position of the paddle may collide with the ball.
        /// If so, then the movement of the paddle is stopped until the ball moves out of the way.
        /// </summary>
        /// Direction at which the paddle has to be moved.
        /// <param name="direction"></param>
        public void MovePaddle(Direction direction)
        {
            if (direction == Direction.Default) return;
            var oldPosition = _gameCanvas.Paddle.Position;
            var ballPosition = _gameCanvas.Ball.Position;
            double newXCoordinate = direction == Direction.Left ? oldPosition.X - 5 : oldPosition.X + 5;
            var newPaddlePosition = new Point(newXCoordinate, oldPosition.Y);

            if (_ballCollision.WithPaddle.Horizontal(ballPosition, newPaddlePosition)
                    && _ballCollision.WithPaddle.Vertical(ballPosition, newPaddlePosition)) return;
            if (newPaddlePosition.X < _gameCanvas.Width - _gameCanvas.Paddle.Width && newPaddlePosition.X > 0)
                SetPosition(_gameCanvas.Paddle.Shape, newPaddlePosition);
        }
        
        /// <summary>
        /// Determines if the ball collides with any brick.
        /// If so, the brick is destroyed.
        /// </summary>
        /// <param name="newPosition">New position of the ball.</param>
        /// <param name="oldPosition">Old position of the ball.</param>
        /// <param name="bounceDirection">Direction at which the ball would bounce off the brick.</param>
        /// <returns></returns>
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
                    CheckBallAccelerationConditions(brick);
                    _gameCanvas.DestroyBrick(brick);
                    return true;
                }
            }
            return false;
        }
        
        /// <summary>
        /// Determines if the ball collides with the paddle.
        /// If so, distance from center of top horizontal edge
        /// of the paddle is calculated for ball to bounce.
        /// </summary>
        /// <param name="newPosition">New position of the ball.</param>
        /// <param name="oldPosition">Old position of the ball.</param>
        /// <param name="distanceFromCenterOfPaddle">Distance from the point where ball touches
        /// the paddle to the center of vertical edge of paddle.</param>
        /// <param name="bounceDirection">Direction at which the ball has to bounce off the paddle.</param>
        /// <returns></returns>
        private bool PaddleCollision(Point newPosition, Point oldPosition, out double distanceFromCenterOfPaddle, out Direction bounceDirection)
        {
            var centerOfPaddleX = _gameCanvas.Paddle.Position.X + _gameCanvas.Paddle.Width / 2;
            var paddlePosition = _gameCanvas.Paddle.Position;
            distanceFromCenterOfPaddle = 0;
            bounceDirection = Direction.Default;
            if (_ballCollision.WithPaddle.VerticalAndHorizontal(newPosition, paddlePosition))
            {
                if (_ballCollision.WithPaddle.OnlyVertical(oldPosition))
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
        
        /// <summary>
        /// Determines if the ball collides with the walls.
        /// If so, direction at which the ball has to bounce is determined.
        /// </summary>
        /// <param name="newPosition"></param>
        /// <param name="bounceDirection"></param>
        /// <returns></returns>
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
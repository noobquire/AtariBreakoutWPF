using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AtariBreakoutWPF
{
    public sealed class GameLogic
    {
        private GameCanvas _canvas;
        private GameCanvasBuilder _gameCanvasBuilder;

        public GameLogic(Canvas gameCanvas)
        {
            _gameCanvasBuilder = new GameCanvasBuilder(gameCanvas);
            _gameCanvasBuilder.AddPaddle();
            _gameCanvasBuilder.AddBricks();
            _gameCanvasBuilder.AddBall();
            _canvas = _gameCanvasBuilder.Build();
        }

        public event EventHandler<ScoreChangedEventArgs> ScoreChanged;
        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<BallDestroyedEventArgs> BallDestroyed;

        public void SetBallSpeed(int newSpeed)
        {
            _canvas.Ball.Speed = newSpeed;
        }
        private void SetPosition(UIElement element, Point position)
        {
            Canvas.SetTop(element, position.Y);
            Canvas.SetLeft(element, position.X);
        }

        public void MoveBall()
        {
            var oldPosition = _canvas.Ball.Position;
            var newPosition = new Point(oldPosition.X + _canvas.Ball.MoveVector.X * _canvas.Ball.Speed,
                oldPosition.Y + _canvas.Ball.MoveVector.Y * _canvas.Ball.Speed);

            CheckWallCollision(newPosition);
            CheckPaddleCollision(newPosition, oldPosition);
            CheckBrickCollision(newPosition, oldPosition);
        }

        private void CheckBrickCollision(Point newPosition, Point oldPosition)
        {
            for (var i = 0; i < _canvas.Bricks.Count; i++)
            {
                var brick = _canvas.Bricks[i];
                if (CollidesWithBrickHorizontal(newPosition, brick) && CollidesWithBrickVertical(newPosition, brick))
                {
                    if (CollidesWithBrickHorizontal(oldPosition, brick) &&
                        !CollidesWithBrickVertical(oldPosition, brick))
                        _canvas.Ball.Bounce(Direction.Vertical);
                    else if (CollidesWithBrickVertical(oldPosition, brick) &&
                             !CollidesWithBrickHorizontal(oldPosition, brick))
                        _canvas.Ball.Bounce(Direction.Horizontal);

                    _gameCanvasBuilder.DestroyBrick(brick);
                }
            }
        }

        private bool CollidesWithBrickVertical(Point newPosition, Brick brick)
        {
            return newPosition.X >= brick.Position.X - _canvas.Ball.Shape.Width &&
                   newPosition.X <= brick.Position.X + Brick.Width;
        }

        private bool CollidesWithBrickHorizontal(Point newPosition, Brick brick)
        {
            return newPosition.Y >= brick.Position.Y - _canvas.Ball.Shape.Width &&
                   newPosition.Y <= brick.Position.Y + Brick.Height;
        }

        private void CheckPaddleCollision(Point newPosition, Point oldPosition)
        {
            if (BallCollidesWithPaddle(newPosition))
            {
                var centerOfPaddleX = _canvas.Paddle.Position.X + _canvas.Paddle.Width / 2;
                double distanceFromCenterOfPaddle;

                if (BallCollidesWithPaddleVertically(oldPosition))
                {
                    distanceFromCenterOfPaddle = _canvas.Paddle.Width / 2;

                    _canvas.Ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Vertical, _canvas.Paddle.Width);
                }
                else if (BallCollidesWithPaddleHorizontally(oldPosition))
                {
                    var centerOfBallX = _canvas.Ball.Position.X + _canvas.Ball.Shape.Width / 2;
                    distanceFromCenterOfPaddle = Math.Abs(centerOfPaddleX - centerOfBallX);
                    if (distanceFromCenterOfPaddle > _canvas.Paddle.Width / 2) distanceFromCenterOfPaddle = _canvas.Paddle.Width / 2;

                    _canvas.Ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Horizontal, _canvas.Paddle.Width);
                }

                _canvas.Paddle.Shape.Fill = Brushes.GreenYellow;
            }
            else
            {
                _canvas.Paddle.Shape.Fill = Brushes.DimGray;
            }
        }

        private bool BallCollidesWithPaddleHorizontally(Point oldPosition)
        {
            return CollidesWithPaddleVertical(oldPosition, _canvas.Paddle.Position) &&
                   !CollidesWithPaddleHorizontal(oldPosition, _canvas.Paddle.Position);
        }

        private bool BallCollidesWithPaddleVertically(Point oldPosition)
        {
            return CollidesWithPaddleHorizontal(oldPosition, _canvas.Paddle.Position) &&
                   !CollidesWithPaddleVertical(oldPosition, _canvas.Paddle.Position);
        }

        private bool BallCollidesWithPaddle(Point newPosition)
        {
            return CollidesWithPaddleHorizontal(newPosition, _canvas.Paddle.Position) &&
                   CollidesWithPaddleVertical(newPosition, _canvas.Paddle.Position);
        }

        private bool CollidesWithPaddleVertical(Point ballPosition, Point paddlePosition)
        {
            return ballPosition.X >= paddlePosition.X - _canvas.Ball.Shape.Width &&
                   ballPosition.X <= paddlePosition.X + _canvas.Paddle.Width;
        }

        private bool CollidesWithPaddleHorizontal(Point ballPosition, Point paddlePosition)
        {
            return ballPosition.Y >= paddlePosition.Y - _canvas.Ball.Shape.Height &&
                   ballPosition.Y <= paddlePosition.Y + _canvas.Paddle.Height;
        }

        private void CheckWallCollision(Point newPosition)
        {
            var outBoundsVertical = newPosition.X <= 0 || newPosition.X >= _canvas.Width - _canvas.Ball.Shape.Width;
            var outBoundsTop = newPosition.Y <= 0;
            var outBoundsBottom = newPosition.Y >= _canvas.Height - _canvas.Ball.Shape.Height;

            if (outBoundsTop)
            {
                _canvas.Ball.Bounce(Direction.Horizontal);
            }
            else if (outBoundsVertical)
            {
                _canvas.Ball.Bounce(Direction.Vertical);
            }
            else if (outBoundsBottom)
            {
                _gameCanvasBuilder.DestroyBall();
                _gameCanvasBuilder.AddBall();
            }
            else
            {
                SetPosition(_canvas.Ball, newPosition);
            }
        }

        // TODO: extract class MovementLogic
                public void MovePaddle(Direction direction)
                {
                    var oldPosition = _canvas.Paddle.Position;
                    if (direction == Direction.Left)
                    {
                        var newPositon = new Point(oldPosition.X - 3,
                            oldPosition.Y);
                        if (CollidesWithPaddleHorizontal(_canvas.Ball.Position, newPositon)
                            && CollidesWithPaddleVertical(_canvas.Ball.Position, newPositon)) return;
                        if (newPositon.X < _canvas.Width - _canvas.Paddle.Width && newPositon.X > 0)
                            SetPosition(_canvas.Paddle.Shape, newPositon);
                    }
                    else if (direction == Direction.Right)
                    {
                        var newPositon = new Point(oldPosition.X + 3,
                            oldPosition.Y);
                        if (CollidesWithPaddleHorizontal(_canvas.Ball.Position, newPositon)
                            && CollidesWithPaddleVertical(_canvas.Ball.Position, newPositon)) return;
                        if (newPositon.X < _canvas.Width - _canvas.Paddle.Width && newPositon.X > 0)
                            SetPosition(_canvas.Paddle.Shape, newPositon);
                    }
                }

        private void OnGameOver(GameOverEventArgs e)
        {
            GameOver?.Invoke(this, e);
        }

        private void OnBallDestroyed(BallDestroyedEventArgs e)
        {
            BallDestroyed?.Invoke(this, e);
        }

        private void OnScoreChanged(ScoreChangedEventArgs e)
        {
            ScoreChanged?.Invoke(this, e);
        }
    }
}
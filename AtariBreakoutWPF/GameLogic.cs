using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AtariBreakoutWPF
{
    class GameLogic
    {
        private Canvas GameCanvas { get; }
        private int Score { get; set; }
        public BouncyBall Ball; // TODO: set back to private
        private Paddle _paddle;
        private readonly IList<Brick> _bricks;
        private readonly double _width;
        private readonly double _height;
        private int _ballCount;

        public event EventHandler<ScoreChangedEventArgs> ScoreChanged;
        public event EventHandler<GameOverEventArgs> GameOver;
        public event EventHandler<BallDestroyedEventArgs> BallDestroyed; 

        public void SetBallSpeed(int newSpeed)
        {
            Ball.Speed = newSpeed;
        }
        public GameLogic(Canvas gameCanvas)
        {
            this.GameCanvas = gameCanvas;
            Score = 0;
            _ballCount = 5;
            _bricks = new List<Brick>();
            _width = gameCanvas.Width;
            _height = gameCanvas.Height;

            AddPaddle();
            AddBall();
            AddBricks();
        }

        private void AddPaddle()
        {
            _paddle = new Paddle(160);
            SetPosition(_paddle.Shape, GameCanvas.Width / 2 - _paddle.Width / 2, GameCanvas.Height - 60);
            GameCanvas.Children.Add(_paddle.Shape);
        }

        private void AddBricks()
        {
            AddBricksRow(Brushes.Red, 30, 7);
            AddBricksRow(Brushes.Red, 60, 7);
            AddBricksRow(Brushes.Yellow, 90, 5);
            AddBricksRow(Brushes.Yellow, 120, 5);
            AddBricksRow(Brushes.Green, 150, 3);
            AddBricksRow(Brushes.Green, 180, 3);
        }

        private void SetPosition(UIElement element, double x, double y)
        {
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, x);
        }
        private void SetPosition(UIElement element, Point position)
        {
            Canvas.SetTop(element, position.Y);
            Canvas.SetLeft(element, position.X);
        }

        public void MoveBall()
        {
            Point oldPosition = Ball.Position;
            Point newPosition = new Point(oldPosition.X + Ball.MoveVector.X * Ball.Speed, oldPosition.Y + Ball.MoveVector.Y * Ball.Speed);

            CheckWallCollision(newPosition);
            CheckPaddleCollision(newPosition, oldPosition);
            CheckBrickCollision(newPosition, oldPosition);
        }

        private void CheckBrickCollision(Point newPosition, Point oldPosition) 
        {
            for(int i = 0; i < _bricks.Count; i++)
            {
                Brick brick = _bricks[i];
                if (CollidesWithBrickHorizontal(newPosition, brick) && CollidesWithBrickVertical(newPosition, brick))
                {
                    if(CollidesWithBrickHorizontal(oldPosition, brick) && !CollidesWithBrickVertical(oldPosition, brick))
                    {
                        Ball.Bounce(Direction.Vertical);
                    } else if (CollidesWithBrickVertical(oldPosition, brick) && !CollidesWithBrickHorizontal(oldPosition, brick))
                    {
                        Ball.Bounce(Direction.Horizontal);
                    }
                    DestroyBrick(brick);
                }
            }
        }

        private bool CollidesWithBrickVertical(Point newPosition, Brick brick)
        {
            return newPosition.X >= brick.Position.X - Ball.Ball.Width &&
                   newPosition.X <= brick.Position.X + Brick.Width;
        }

        private bool CollidesWithBrickHorizontal(Point newPosition, Brick brick)
        {
            return newPosition.Y >= brick.Position.Y - Ball.Ball.Width &&
                   newPosition.Y <= brick.Position.Y + Brick.Height;
        }

        private void CheckPaddleCollision(Point newPosition, Point oldPosition)
        {
            double centerOfPaddleX = _paddle.Position.X - (_paddle.Width / 2);
            double distanceFromCenterOfPaddle =
                Math.Abs(centerOfPaddleX - newPosition.X - Ball.Ball.Width); // TODO: fix dfcop calculation
            if (CollidesWithPaddleHorizontal(newPosition, _paddle.Position) && CollidesWithPaddleVertical(newPosition, _paddle.Position))
            {
                if (CollidesWithPaddleHorizontal(oldPosition, _paddle.Position) && !CollidesWithPaddleVertical(oldPosition, _paddle.Position))
                {
                    Ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Vertical, _paddle.Width);
                }
                else if (CollidesWithPaddleVertical(oldPosition, _paddle.Position) && !CollidesWithPaddleHorizontal(oldPosition, _paddle.Position))
                {
                    Ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Horizontal, _paddle.Width);
                }

                _paddle.Shape.Fill = Brushes.GreenYellow;
            }
            else
            {
                _paddle.Shape.Fill = Brushes.DimGray;
            }
        }

        private bool CollidesWithPaddleVertical(Point ballPosition, Point paddlePosition)
        {
            return ballPosition.X >= paddlePosition.X - Ball.Ball.Width && ballPosition.X <= paddlePosition.X + _paddle.Width;
        }

        private bool CollidesWithPaddleHorizontal(Point ballPosition, Point paddlePosition)
        {
            return ballPosition.Y >= paddlePosition.Y - Ball.Ball.Height && ballPosition.Y <= paddlePosition.Y + _paddle.Height;
        }

        private void CheckWallCollision(Point newPosition)
        {
            bool outBoundsVertical = newPosition.X <= 0 || newPosition.X >= _width - Ball.Ball.Width;
            bool outBoundsTop = newPosition.Y <= 0;
            bool outBoundsBottom = newPosition.Y >= _height - Ball.Ball.Height;

            if (outBoundsTop)
            {
                Ball.Bounce(Direction.Horizontal);
            }
            else if (outBoundsVertical)
            {
                Ball.Bounce(Direction.Vertical);
            }
            else if (outBoundsBottom)
            {
                Ball.Bounce(Direction.Horizontal);
                // TODO: destroy ball if it is out of bottom bound
                //DestroyBall();
                //AddBall();
            }
            else
            {
                SetPosition(Ball, newPosition);
            }
        }

        private void AddBall()
        {
            if (_ballCount > 0)
            {
                Ball = new BouncyBall();
                GameCanvas.Children.Add(Ball);
                SetPosition(Ball, GameCanvas.Width / 2 - Ball.Ball.Width / 2, GameCanvas.Height - 100);
            }
            else
            {
                
                MessageBox.Show($"Your score: {Score}", "Game Over", MessageBoxButton.OK); // TODO: finish and restart game after this message
            } // TODO: message should probably be moved to MainWindow.xaml.cs

        }

        private void DestroyBall()
        {
            GameCanvas.Children.Remove(Ball.Ball);
            Ball.Ball = null;
            Ball = null;
            _ballCount--;
        }
        private void DestroyBrick(Brick brick)
        {
            Score += brick.ScoreForDestruction;
            GameCanvas.Children.Remove(brick.Shape);
            brick.Shape = null;
            _bricks.Remove(brick);
            ScoreChanged?.Invoke(this, new ScoreChangedEventArgs(Score));
        }

        public void AddBricksRow(Brush colorBrush, int height, int score)
        {
            // TODO: algorithm to generate bricks depending on gameCanvas size
            int numberOfBricks = 10;
            int distanceBetweenBricks = 10;
            int currentX = 5;
            for (int i = 0; i < numberOfBricks; i++)
            {
                Brick brick = new Brick(colorBrush,score);
                SetPosition(brick, currentX, height);
                GameCanvas.Children.Add(brick.Shape);
                _bricks.Add(brick);
                currentX += distanceBetweenBricks + Brick.Width;
            }
        }

        public void MovePaddle(Direction direction)
        { 
            Point oldPosition = _paddle.Position;
            if (direction == Direction.Left)
            {
                Point newPositon = new Point(oldPosition.X - 3,
                    oldPosition.Y);
                if(CollidesWithPaddleHorizontal(Ball.Position, newPositon) 
                   && CollidesWithPaddleVertical(Ball.Position, newPositon)) return;
                if (newPositon.X < GameCanvas.Width - _paddle.Width && newPositon.X > 0 )
                {
                    SetPosition(_paddle.Shape, newPositon);
                }

            }
            else if (direction == Direction.Right)
            {
                Point newPositon = new Point(oldPosition.X + 3,
                    oldPosition.Y);
                if (CollidesWithPaddleHorizontal(Ball.Position, newPositon)
                    && CollidesWithPaddleVertical(Ball.Position, newPositon)) return;
                if (newPositon.X < GameCanvas.Width - _paddle.Width && newPositon.X > 0)
                {
                    SetPosition(_paddle.Shape, newPositon);
                }
            }
        }

        protected virtual void OnGameOver(GameOverEventArgs e)
        {
            GameOver?.Invoke(this, e);
        }

        protected virtual void OnBallDestroyed(BallDestroyedEventArgs e)
        {
            BallDestroyed?.Invoke(this, e);
        }
    }
}

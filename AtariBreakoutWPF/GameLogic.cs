using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace AtariBreakoutWPF
{
    class GameLogic
    {
        private Canvas gameCanvas { get; }
        public int Score { get; private set; }
        public BouncyBall ball; // TODO: set back to private
        private Paddle paddle;
        private readonly IList<Brick> bricks;
        private double width, height;
        private int ballCount;

        public void SetBallSpeed(int newSpeed)
        {
            ball.Speed = newSpeed;
        }
        public GameLogic(Canvas gameCanvas)
        {
            this.gameCanvas = gameCanvas;
            Score = 0;
            ballCount = 5;
            bricks = new List<Brick>();
            width = gameCanvas.Width;
            height = gameCanvas.Height;

            AddPaddle();
            AddBall();
            AddBricks();
        }

        private void AddPaddle()
        {
            paddle = new Paddle(80);
            SetPosition(paddle.Shape, gameCanvas.Width / 2 - paddle.Width / 2, gameCanvas.Height - 60);
            gameCanvas.Children.Add(paddle.Shape);
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
            Point oldPosition = ball.Position;
            Point newPosition = new Point(oldPosition.X + ball.MoveVector.X * ball.Speed, oldPosition.Y + ball.MoveVector.Y * ball.Speed);

            CheckWallCollision(newPosition);
            CheckPaddleCollision(newPosition, oldPosition);
            CheckBrickCollision(newPosition, oldPosition);
        }

        private void CheckBrickCollision(Point newPosition, Point oldPosition) 
        {
            for(int i = 0; i < bricks.Count; i++)
            {
                Brick brick = bricks[i];
                if (CollidesWithBrickHorizontal(newPosition, brick) && CollidesWithBrickVertical(newPosition, brick))
                {
                    if(CollidesWithBrickHorizontal(oldPosition, brick) && !CollidesWithBrickVertical(oldPosition, brick))
                    {
                        ball.Bounce(Direction.Vertical);
                    } else if (CollidesWithBrickVertical(oldPosition, brick) && !CollidesWithBrickHorizontal(oldPosition, brick))
                    {
                        ball.Bounce(Direction.Horizontal);
                    }
                    DestroyBrick(brick);
                }
            }
        }

        private bool CollidesWithBrickVertical(Point newPosition, Brick brick)
        {
            return newPosition.X >= brick.Position.X - ball.Ball.Width &&
                   newPosition.X <= brick.Position.X + Brick.Width;
        }

        private bool CollidesWithBrickHorizontal(Point newPosition, Brick brick)
        {
            return newPosition.Y >= brick.Position.Y - ball.Ball.Width &&
                   newPosition.Y <= brick.Position.Y + Brick.Height;
        }

        private void CheckPaddleCollision(Point newPosition, Point oldPosition)
        {
            double centerOfPaddleX = paddle.Position.X - (paddle.Width / 2);
            double distanceFromCenterOfPaddle =
                Math.Abs(centerOfPaddleX - newPosition.X - ball.Ball.Width); // TODO: think about it more
            if (CollidesWithPaddleHorizontal(newPosition) && CollidesWithPaddleVertical(newPosition))
            {
                if (CollidesWithPaddleHorizontal(oldPosition) && !CollidesWithPaddleVertical(oldPosition))
                {
                    ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Vertical);
                }
                else if (CollidesWithPaddleVertical(oldPosition) && !CollidesWithPaddleHorizontal(oldPosition))
                {
                    ball.BounceOffPaddle(distanceFromCenterOfPaddle, Direction.Horizontal);
                }

                paddle.Shape.Fill = Brushes.GreenYellow;
            }
            else
            {
                paddle.Shape.Fill = Brushes.DimGray;
            }
        }

        private bool CollidesWithPaddleVertical(Point ballPosition)
        {
            return ballPosition.X >= paddle.Position.X - ball.Ball.Width && ballPosition.X <= paddle.Position.X + paddle.Width;
        }

        private bool CollidesWithPaddleHorizontal(Point ballPosition)
        {
            return ballPosition.Y >= paddle.Position.Y - ball.Ball.Height && ballPosition.Y <= paddle.Position.Y + paddle.Height;
        }

        private void CheckWallCollision(Point newPosition)
        {
            bool outBoundsVertical = newPosition.X <= 0 || newPosition.X >= width - ball.Ball.Width;
            bool outBoundsTop = newPosition.Y <= 0;
            bool outBoundsBottom = newPosition.Y >= height - ball.Ball.Height;

            if (outBoundsTop)
            {
                ball.Bounce(Direction.Horizontal);
            }
            else if (outBoundsVertical)
            {
                ball.Bounce(Direction.Vertical);
            }
            else if (outBoundsBottom)
            {
                ball.Bounce(Direction.Horizontal);
                // TODO: destroy ball if it is out of bottom bound
                //DestroyBall();
                //AddBall();
            }
            else
            {
                SetPosition(ball, newPosition);
            }
        }

        private void AddBall()
        {
            if (ballCount > 0)
            {
                ball = new BouncyBall();
                gameCanvas.Children.Add(ball);
                SetPosition(ball, gameCanvas.Width / 2 - ball.Ball.Width / 2, gameCanvas.Height - 100);
            }
            else
            {
                MessageBox.Show($"Your score: {Score}", "Game Over", MessageBoxButton.OK); // TODO: finish and restart game after this message
            }

        }

        private void DestroyBall()
        {
            gameCanvas.Children.Remove(ball.Ball);
            ball.Ball = null;
            ball = null;
            ballCount--;
        }
        private void DestroyBrick(Brick brick)
        {
            Score += brick.ScoreForDestruction;
            gameCanvas.Children.Remove(brick.Shape);
            brick.Shape = null;
            bricks.Remove(brick);

        }

        public void AddBricksRow(Brush colorBrush, int height, int score)
        {
            // TODO algorithm to generate bricks depending on gameCanvas size
            int numberOfBricks = 10;
            int distanceBetweenBricks = 10;
            int currentX = 5;
            for (int i = 0; i < numberOfBricks; i++)
            {
                Brick brick = new Brick(colorBrush,score);
                SetPosition(brick, currentX, height);
                gameCanvas.Children.Add(brick.Shape);
                bricks.Add(brick);
                currentX += distanceBetweenBricks + Brick.Width;
            }
        }

        public void MovePaddle(Direction direction)
        { // TODO: add check for collision with ball
            Point oldPosition = paddle.Position;
            if (direction == Direction.Left)
            {

                Point newPositon = new Point(oldPosition.X - 3,
                    oldPosition.Y);
                if (newPositon.X < gameCanvas.Width - paddle.Width && newPositon.X > 0)
                {
                    SetPosition(paddle.Shape, newPositon);
                }

            }
            else if (direction == Direction.Right)
            {
                Point newPositon = new Point(oldPosition.X + 3,
                    oldPosition.Y);
                if (newPositon.X < gameCanvas.Width - paddle.Width && newPositon.X > 0)
                {
                    SetPosition(paddle.Shape, newPositon);
                }
            }
        }

    }
}

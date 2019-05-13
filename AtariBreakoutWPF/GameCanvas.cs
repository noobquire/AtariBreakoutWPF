using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace AtariBreakoutWPF
{
    public class GameCanvas
    {
        public readonly IList<Brick> Bricks = new List<Brick>();
        public readonly Canvas Canvas;
        public BouncyBall Ball;

        public Paddle Paddle;

        public GameCanvas(Canvas canvas)
        {
            Canvas = canvas;
            BallCount = 5;
        }

        public double Height => Canvas.Height;
        public double Width => Canvas.Width;

        public int Score { get; private set; }
        /// <summary>
        /// Amount of bouncy balls that player can destroy before losing
        /// </summary>
        public int BallCount { get; private set; }

        public void AddBall()
        {
            if (BallCount <= 0)
            {
                OnGameOver(new GameOverEventArgs(Score));
                return;
            }

            Ball = new BouncyBall(new Vector(0, -1), 3);
            Canvas.Children.Add(Ball.Shape);
            Utility.SetPosition(Ball.Shape, Paddle.Position.X +
                                            Paddle.Width / 2 - Ball.Shape.Width, Height - 100);
        }

        public void DestroyBall()
        {
            Ball.HitCount = 0;
            Canvas.Children.Remove(Ball.Shape);
            Ball.Shape = null;
            BallCount--;
            OnBallDestroyed(new BallDestroyedEventArgs(BallCount));
        }

        public void DestroyBrick(Brick brick)
        {
            Ball.HitCount++;
            CheckBallAcceleration(brick);

            Canvas.Children.Remove(brick.Shape);
            Bricks.Remove(brick);
            brick.Shape = null;
            Score += brick.ScoreForDestruction;
            OnScoreChanged(new ScoreChangedEventArgs(Score));
        }

        private void CheckBallAcceleration(Brick brick)
        {
            if (!Ball.RedBrickHit && brick.Color == Brick.BrickColor.Red)
            {
                Ball.RedBrickHit = true;
            }

            if (!Ball.OrangeBrickHit && brick.Color == Brick.BrickColor.Orange)
            {
                Ball.OrangeBrickHit = true;
            }
            if (Ball.HitCount == 4 || Ball.HitCount == 12) Ball.Speed += BouncyBall.Acceleration;
        }

        public event EventHandler<ScoreChangedEventArgs> ScoreChanged;

        private void OnScoreChanged(ScoreChangedEventArgs e)
        {
            ScoreChanged?.Invoke(this, e);
        }

        public event EventHandler<GameOverEventArgs> GameOver;

        private void OnGameOver(GameOverEventArgs e)
        {
            GameOver?.Invoke(this, e);
        }

        public event EventHandler<BallDestroyedEventArgs> BallDestroyed;

        private void OnBallDestroyed(BallDestroyedEventArgs e)
        {
            BallDestroyed?.Invoke(this, e);
        }
    }
}
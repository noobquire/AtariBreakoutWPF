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
            Canvas.Children.Remove(Ball.Shape);
            Ball.Shape = null;
            BallCount--;
            OnBallDestroyed(new BallDestroyedEventArgs(BallCount));
        }

        public void DestroyBrick(Brick brick)
        {
            // TODO: Increase ball speed when ball first hits yellow and red bricks etc
            Canvas.Children.Remove(brick.Shape);
            Bricks.Remove(brick);
            brick.Shape = null;
            Score += brick.ScoreForDestruction;
            OnScoreChanged(new ScoreChangedEventArgs(Score));
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
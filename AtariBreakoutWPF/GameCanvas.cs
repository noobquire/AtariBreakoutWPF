using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    // TODO: write basic documentation for GameCanvas
    /// <summary>
    /// Represents a game field on which ball 
    /// </summary>
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

            var ballShape = (Ellipse)Canvas.FindName("BallEllipse");
            Ball = new BouncyBall(new Vector(0, -1), ballShape, 3);
            //Canvas.Children.Add(Ball.Shape);
            Utility.SetPosition(Ball.Shape, Paddle.Position.X +
                                            Paddle.Width / 2 - Ball.Shape.Width, Height - 100);
        }

        /// <summary>
        /// Removes bouncy ball from the canvas, and destroys it
        /// </summary>
        public void DestroyBall()
        {
            //Canvas.Children.Remove(Ball.Shape);
            //Ball.Shape = null;
            BallCount--;
            OnBallDestroyed(new BallDestroyedEventArgs(BallCount));
        }

        /// <summary>
        /// Destroys specified brick on the canvas, and depending on its color increases player's score by certain amount:
        /// </summary>
        /// <param name="brick"></param>
        public void DestroyBrick(Brick brick)
        {
            Ball.HitCount++;

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
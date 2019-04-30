using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    public sealed class BouncyBall
    {
        public Vector MoveVector { get; private set; }
        public Ellipse Ball { get; set; }
        public int Speed { get; set; }
        public BouncyBall(Vector moveVector, Ellipse ball, int speed)
        {
            MoveVector = moveVector;
            Ball = ball;
            Speed = speed;
        }

        ~BouncyBall()
        {
            Ball = null;
        }

        public Point Position => new Point((double)Ball.GetValue(Canvas.LeftProperty), (double)Ball.GetValue(Canvas.TopProperty));

        public void Bounce(Direction direction)
        {
            if (direction == Direction.Horizontal)
            {
                MoveVector = new Vector(MoveVector.X, -MoveVector.Y);
            }

            if (direction == Direction.Vertical)
            {
                MoveVector = new Vector(-MoveVector.X, MoveVector.Y);
            }
        }

        public BouncyBall()
        {
            MoveVector = new Vector(1, 1);
            Ball = new Ellipse
            {
                Height = 20,
                Width = 20,
                StrokeThickness = 2,
                Stroke = Brushes.DarkCyan,
                Fill = Brushes.DarkRed,

            };
            Speed = 5;
        }

        public static implicit operator Ellipse(BouncyBall ball)
        {
            return ball.Ball;
        }

        public void BounceOffPaddle(double distanceFromCenterOfPaddle, Direction direction)
        {
            Bounce(direction); // TODO: diffrent angles depending on dfcop
        }
    }
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    public sealed class BouncyBall
    {
        public Vector MoveVector { get; private set; }
        public Ellipse Shape { get; set; }
        public int Speed { get; set; }

        public Point Position => new Point((double) Shape.GetValue(Canvas.LeftProperty),
            (double) Shape.GetValue(Canvas.TopProperty));

        public BouncyBall(Vector moveVector, int speed)
        {
            MoveVector = moveVector;
            Shape = new Ellipse
            {
                Height = 20,
                Width = 20,
                StrokeThickness = 2,
                Stroke = Brushes.DarkCyan,
                Fill = Brushes.DarkRed
            };
            Speed = speed;
        }

        public BouncyBall()
        {
            MoveVector = new Vector(1, 1);
            Shape = new Ellipse
            {
                Height = 20,
                Width = 20,
                StrokeThickness = 2,
                Stroke = Brushes.DarkCyan,
                Fill = Brushes.DarkRed
            };
            Speed = 5;
        }

        
        ~BouncyBall()
        {
            Shape = null;
        }

        public void Bounce(Direction direction)
        {
            if (direction == Direction.Horizontal) MoveVector = new Vector(MoveVector.X, -MoveVector.Y);

            if (direction == Direction.Vertical) MoveVector = new Vector(-MoveVector.X, MoveVector.Y);
        }

        public static implicit operator Ellipse(BouncyBall ball)
        {
            return ball.Shape;
        }

        public void BounceOffPaddle(double distanceFromCenterOfPaddle, Direction direction, double paddleWidth)
        {
            var coefficient = 1 - distanceFromCenterOfPaddle / (paddleWidth / 2); // must be from 0 to 1
            var i = new Vector(1, 0);
            var fallAngle = Math.Abs(Vector.AngleBetween(i, -MoveVector));
            var bounceAngle = fallAngle >= 90
                ? 45 * (coefficient + 1)
                : 45 * (3 - coefficient);

            var bounceAngleInRad = bounceAngle * Math.PI / 180;
            Vector bounceVector;

            if (direction == Direction.Horizontal)
            {
                var bounceVectorX = Math.Cos(bounceAngleInRad);
                var bounceVectorY = -Math.Sin(bounceAngleInRad);

                bounceVector = new Vector(bounceVectorX, bounceVectorY);
#if DEBUG
                Debug.Assert(coefficient >= 0 && coefficient <= 1, "wrongly calculated bounce coefficient");
                Console.WriteLine("Bouncing off paddle horizontally");
                Console.WriteLine($"dfcop: {distanceFromCenterOfPaddle}");
                Console.WriteLine($"cofficient: {coefficient}");
                Console.WriteLine($"fall angle: {fallAngle}");
                Console.WriteLine($"bounce angle: {bounceAngle}");
                Console.WriteLine();
#endif
            }
            else
            {
                var bounceVectorX = -Math.Cos(bounceAngleInRad);
                var bounceVectorY = Math.Sin(bounceAngleInRad);

                bounceVector = new Vector(bounceVectorX, bounceVectorY);
            }

            MoveVector = bounceVector;
        }
    }
}
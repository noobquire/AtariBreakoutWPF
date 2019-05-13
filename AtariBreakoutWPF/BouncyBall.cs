using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    public sealed class BouncyBall
    {
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

        public const int Acceleration = 1;

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
            Speed = 2;
        }
        /// <summary>
        /// Defines directions in which the ball is moving
        /// </summary>
        public Vector MoveVector { get; private set; }

        /// <summary>
        /// Actual shape of the ball which is added on the canvas
        /// </summary>
        public Ellipse Shape { get; set; }

        /// <summary>
        /// Speed at which the ball moves
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// Amount of bricks the ball destroyed
        /// </summary>
        public int HitCount { get; set; }

        private int _hitCount;

        /// <summary>
        /// Defines if at least one orange brick was destroyed by the ball
        /// </summary>
        public bool OrangeBrickHit { get; set; }
        /// <summary>
        /// Defines if at least one red brick was destroyed by the ball
        /// </summary>
        public bool RedBrickHit { get; set; }
        /// <summary>
        /// Current position of the ball on game canvas
        /// </summary>
        public Point Position => Utility.Position(Shape);


        ~BouncyBall()
        {
            Shape = null;
        }
        /// <summary>
        /// Swaps ball movement along one of the axes, fall angle equals bounce angle
        /// </summary>
        /// <param name="direction">Direction of surface from which the ball is bouncing</param>
        public void Bounce(Direction direction)
        {
            if (direction == Direction.Horizontal) MoveVector = new Vector(MoveVector.X, -MoveVector.Y);

            if (direction == Direction.Vertical) MoveVector = new Vector(-MoveVector.X, MoveVector.Y);
        }

        /// <summary>
        /// Swaps ball movement along one of the axes, and changes angle depending on distance from center of paddle
        /// </summary>
        /// <param name="distanceFromCenterOfPaddle">Absolute difference between X coordinates of ball's and paddle's centers. No bigger than half width of the paddle</param>
        /// <param name="direction">Direction of surface from which the ball is bouncing</param>
        /// <param name="paddleWidth">Width of the paddle</param>
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
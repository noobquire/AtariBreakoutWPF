using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    /// <summary>
    /// Represents a game brick placed on a game field
    /// which may be destroyed by the ball.
    /// </summary>
    public sealed class Brick
    {
        public const int Width = 55;
        public const int Height = 20;
        /// <summary>
        /// Score which is added to overall score for destructing this brick
        /// </summary>
        public readonly int ScoreForDestruction;
        /// <summary>
        /// Color of this brick.
        /// The color order is yellow, green, orange, red.
        /// Color determines amount of points for destruction of the brick.
        /// Red and orange bricks accelerate the ball when they are hit.
        /// </summary>
        public readonly BrickColor Color;

        /// <summary>
        /// Actual shape of the brick which is added to the canvas
        /// </summary>
        public Rectangle Shape;

        public Brick(BrickColor color)
        {
            Color = color;
            Brush colorBrush = Brushes.Brown;
            switch (color)
            {
                case BrickColor.Yellow:
                    colorBrush = Brushes.Yellow;
                    break;
                case BrickColor.Green:
                    colorBrush = Brushes.LimeGreen;
                    break;
                case BrickColor.Orange:
                    colorBrush = Brushes.DarkOrange;
                    break;
                case BrickColor.Red:
                    colorBrush = Brushes.Red;
                    break;
            }

            Shape = new Rectangle
            {
                Width = Width,
                Height = Height,
                Fill = colorBrush,
                StrokeThickness = 1,
                Stroke = Brushes.Black,
            };
            ScoreForDestruction = (int) color;
        }

        public enum BrickColor : int
        {
            Yellow = 1,
            Green = 3,
            Orange = 5,
            Red = 7
        }

        public Point Position => Utility.Position(Shape);
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    /// <summary>
    /// Represents a moving platform controlled
    /// by the player, from which the ball
    /// can bounce depending on distance from center of it.
    /// </summary>
    public sealed class Paddle
    {
        /// <summary>
        /// Height of the paddle
        /// </summary>
        public readonly double Height = 20;

        public Paddle(int width)
        {
            Shape = new Rectangle
            {
                Width = width,
                Height = Height,
                StrokeThickness = 2,
                Stroke = Brushes.DarkBlue,
                Fill = Brushes.DimGray
            };
        }

        /// <summary>
        /// Actual shape of the paddle which is added to the canvas
        /// </summary>
        public Rectangle Shape { get; }

        /// <summary>
        /// Width of the paddle
        /// </summary>
        public double Width
        {
            get => Shape.Width;
            set => Shape.Width = value;
        }

        /// <summary>
        /// Position of the paddle on game field
        /// </summary>
        public Point Position => new Point((double) Shape.GetValue(Canvas.LeftProperty),
            (double) Shape.GetValue(Canvas.TopProperty));
    }
}
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    public sealed class Paddle
    {
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

        public Rectangle Shape { get; }

        public double Width
        {
            get => Shape.Width;
            set => Shape.Width = value;
        }

        public Point Position => new Point((double) Shape.GetValue(Canvas.LeftProperty),
            (double) Shape.GetValue(Canvas.TopProperty));
    }
}
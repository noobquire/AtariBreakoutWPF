using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    public sealed class Brick
    {
        public static readonly int Width = 70;
        public static readonly int Height = 20;
        public readonly int ScoreForDestruction;

        public Brick(Brush colorBrush, int score)
        {
            Shape = new Rectangle
            {
                Width = Width,
                Height = Height,
                Fill = colorBrush,
                StrokeThickness = 2,
                Stroke = Brushes.Black
            };
            ScoreForDestruction = score;
        }

        public Rectangle Shape;

        public Point Position => Utility.Position(Shape);
    }
}
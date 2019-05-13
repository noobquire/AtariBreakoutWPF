using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    public sealed class Brick
    {
        public static readonly int Width = 55;
        public static readonly int Height = 20;
        public readonly int ScoreForDestruction;

        public Rectangle Shape;

        public Brick(Brush colorBrush, int score)
        {
            Shape = new Rectangle
            {
                Width = Width,
                Height = Height,
                Fill = colorBrush,
                StrokeThickness = 1,
                Stroke = Brushes.Black,
            };
            ScoreForDestruction = score;
        }

        public Point Position => Utility.Position(Shape);
    }
}
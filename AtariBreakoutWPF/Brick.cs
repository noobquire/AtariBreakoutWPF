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
        public readonly BrickColor Color;


        public Rectangle Shape;

        public Brick(BrickColor color, int score)
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
            ScoreForDestruction = score;
        }
        public enum BrickColor
        {
            Yellow = 1,
            Green = 3,
            Orange = 5,
            Red = 7
        }

        public Point Position => Utility.Position(Shape);
    }
}
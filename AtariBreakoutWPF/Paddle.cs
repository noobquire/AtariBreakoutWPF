using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AtariBreakoutWPF
{
    public sealed class Paddle
    {
        public Rectangle Shape { get; }

        public double Width
        {
            get => Shape.Width;
            set => Shape.Width = value;
        }

        public readonly double Height = 20;

        public Point Position
        {
            get
            {
                return new Point((double)Shape.GetValue(Canvas.LeftProperty), (double)Shape.GetValue(Canvas.TopProperty));
            }
        }

        public Paddle(int width)
        {
            Shape = new Rectangle
            {
                Width = width,
                Height = Height,
                StrokeThickness = 2,
                Stroke = Brushes.DarkBlue,
                Fill = Brushes.DimGray,
            };
        }
    }
}

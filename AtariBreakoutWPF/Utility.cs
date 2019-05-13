using System.Windows;
using System.Windows.Controls;

namespace AtariBreakoutWPF
{
    public static class Utility
    {
        public static void SetPosition(UIElement element, double newX, double newY)
        {
            element.SetValue(Canvas.LeftProperty, newX);
            element.SetValue(Canvas.TopProperty, newY);
        }

        public static void SetPosition(UIElement element, Point newPosition)
        {
            SetPosition(element, newPosition.X, newPosition.Y);
        }

        public static Point Position(UIElement element)
        {
            return new Point((double) element.GetValue(Canvas.LeftProperty),
                (double) element.GetValue(Canvas.TopProperty));
        }
    }
}
using System.Windows;

namespace AtariBreakoutWPF.CollisionDetection
{
    public class BrickCollision
    {
        private GameCanvas _gameCanvas;

        public BrickCollision(GameCanvas gameCanvas)
        {
            _gameCanvas = gameCanvas;
        }
        
        public bool Vertical(Point ballPosition, Brick brick)
        {
            return ballPosition.X >= brick.Position.X - _gameCanvas.Ball.Shape.Width &&
                   ballPosition.X <= brick.Position.X + Brick.Width;
        }

        public bool Horizontal(Point ballPosition, Brick brick)
        {
            return ballPosition.Y >= brick.Position.Y - _gameCanvas.Ball.Shape.Width &&
                   ballPosition.Y <= brick.Position.Y + Brick.Height;
        }

        public bool VerticalAndHorizontal(Point ballPosition, Brick brick)
        {
            return Vertical(ballPosition, brick) && Horizontal(ballPosition, brick);
        }

        public bool OnlyHorizontal(Point ballPosition, Brick brick)
        {
            return Horizontal(ballPosition, brick) && !Vertical(ballPosition, brick);
        }

        public bool OnlyVertical(Point ballPosition, Brick brick)
        {
            return Vertical(ballPosition, brick) && !Horizontal(ballPosition, brick);
        }
    }
}
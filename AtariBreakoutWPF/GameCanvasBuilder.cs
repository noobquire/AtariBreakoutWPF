using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace AtariBreakoutWPF
{
    public class GameCanvasBuilder
    {
        private GameCanvas _gameCanvas;

        public GameCanvasBuilder(Canvas canvas)
        {
            _gameCanvas = new GameCanvas(canvas);
        }

        private void SetPosition(UIElement element, double x, double y)
        {
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, x);
        }

        public void AddPaddle()
        {
            _gameCanvas.Paddle = new Paddle(160);
            SetPosition(_gameCanvas.Paddle.Shape, _gameCanvas.Width / 2 - _gameCanvas.Paddle.Width / 2, _gameCanvas.Height - 60);
            _gameCanvas.Canvas.Children.Add(_gameCanvas.Paddle.Shape);
        }

        public void AddBricks()
        {
            AddBricksRow(Brushes.Red, 30, 7);
            AddBricksRow(Brushes.Red, 60, 7);
            AddBricksRow(Brushes.Yellow, 90, 5);
            AddBricksRow(Brushes.Yellow, 120, 5);
            AddBricksRow(Brushes.Green, 150, 3);
            AddBricksRow(Brushes.Green, 180, 3);
        }

        private void AddBricksRow(Brush colorBrush, int height, int score)
        {
            // TODO: algorithm to generate bricks depending on gameCanvas size
            var numberOfBricks = 10;
            var distanceBetweenBricks = 10;
            var currentX = 5;
            for (var i = 0; i < numberOfBricks; i++)
            {
                var brick = new Brick(colorBrush, score);
                SetPosition(brick, currentX, height);
                _gameCanvas.Canvas.Children.Add(brick.Shape); // todo move to setter
                _gameCanvas.Bricks.Add(brick); 
                currentX += distanceBetweenBricks + Brick.Width;
            }
        }

        public void AddBall()
        {
            _gameCanvas.Ball = new BouncyBall(new Vector(0, -1), 3);
            _gameCanvas.Canvas.Children.Add(_gameCanvas.Ball); // todo move to setter
            SetPosition(_gameCanvas.Ball, _gameCanvas.Paddle.Position.X +
                                          _gameCanvas.Paddle.Width / 2 - _gameCanvas.Ball.Shape.Width, _gameCanvas.Height - 100);
        }

        public void DestroyBall()
        {
            _gameCanvas.Canvas.Children.Remove(_gameCanvas.Ball.Shape);
            _gameCanvas.Ball.Shape = null;
            _gameCanvas.Ball = null;
        }

        public void DestroyBrick(Brick brick)
        {
            // TODO: Increase ball speed when ball first hits yellow and red bricks etc
            _gameCanvas.Canvas.Children.Remove(brick.Shape);
            brick.Shape = null;
            _gameCanvas.Bricks.Remove(brick);
        }

        public GameCanvas Build()
        {
            return _gameCanvas;
        }
    }
}
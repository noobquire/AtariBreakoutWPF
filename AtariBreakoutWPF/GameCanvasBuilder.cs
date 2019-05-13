using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static AtariBreakoutWPF.Utility;

namespace AtariBreakoutWPF
{
    public class GameCanvasBuilder
    {
        private readonly GameCanvas _gameCanvas;

        public GameCanvasBuilder(Canvas canvas)
        {
            _gameCanvas = new GameCanvas(canvas);
        }

        public void AddPaddle()
        {
            _gameCanvas.Paddle = new Paddle(160);
            SetPosition(_gameCanvas.Paddle.Shape, _gameCanvas.Width / 2 - _gameCanvas.Paddle.Width / 2,
                _gameCanvas.Height - 60);
            _gameCanvas.Canvas.Children.Add(_gameCanvas.Paddle.Shape);
        }

        public void AddBricks()
        {
            AddBricksRow(Brushes.Red, 30, 7);
            AddBricksRow(Brushes.Red, 55, 7);
            AddBricksRow(Brushes.DarkOrange, 80, 5);
            AddBricksRow(Brushes.DarkOrange, 105, 5);
            AddBricksRow(Brushes.Green, 130, 3);
            AddBricksRow(Brushes.Green, 155, 3);
            AddBricksRow(Brushes.Yellow, 180, 1);
            AddBricksRow(Brushes.Yellow, 205, 1);
        }

        private void AddBricksRow(Brush colorBrush, int height, int score)
        {
            var numberOfBricks = 14;
            var distanceBetweenBricks = (_gameCanvas.Width - numberOfBricks * Brick.Width)/numberOfBricks;
            double currentX = 1;
            for (var i = 0; i < numberOfBricks; i++)
            {
                var brick = new Brick(colorBrush, score);
                SetPosition(brick.Shape, currentX, height);
                _gameCanvas.Canvas.Children.Add(brick.Shape);
                _gameCanvas.Bricks.Add(brick);
                currentX += distanceBetweenBricks + Brick.Width;
            }
        }

        public void AddBall()
        {
            _gameCanvas.Ball = new BouncyBall(new Vector(0, -1), 3);
            _gameCanvas.Canvas.Children.Add(_gameCanvas.Ball.Shape);
            SetPosition(_gameCanvas.Ball.Shape, _gameCanvas.Paddle.Position.X +
                                                _gameCanvas.Paddle.Width / 2 - _gameCanvas.Ball.Shape.Width,
                _gameCanvas.Height - 100);
        }

        public GameCanvas Build()
        {
            return _gameCanvas;
        }
    }
}
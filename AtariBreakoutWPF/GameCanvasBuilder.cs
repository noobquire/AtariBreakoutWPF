using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static AtariBreakoutWPF.Utility;

namespace AtariBreakoutWPF
{
    public class GameCanvasBuilder
    {
        private GameCanvas _gameCanvas;

        public GameCanvasBuilder(Canvas canvas)
        {
            _gameCanvas = new GameCanvas(canvas);
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
                SetPosition(brick.Shape, currentX, height);
                _gameCanvas.Canvas.Children.Add(brick.Shape); // todo move to setter
                _gameCanvas.Bricks.Add(brick); 
                currentX += distanceBetweenBricks + Brick.Width;
            }
        }

        public void AddBall()
        {
            _gameCanvas.Ball = new BouncyBall(new Vector(0, -1), 3);
            _gameCanvas.Canvas.Children.Add(_gameCanvas.Ball.Shape); // todo move to setter
            SetPosition(_gameCanvas.Ball.Shape, _gameCanvas.Paddle.Position.X +
                                          _gameCanvas.Paddle.Width / 2 - _gameCanvas.Ball.Shape.Width, _gameCanvas.Height - 100);
        }

        public GameCanvas Build()
        {
            return _gameCanvas;
        }
    }
}
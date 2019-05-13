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
            AddBricksRow(Brick.BrickColor.Red, 30, 7);
            AddBricksRow(Brick.BrickColor.Red, 55, 7);
            AddBricksRow(Brick.BrickColor.Orange, 80, 5);
            AddBricksRow(Brick.BrickColor.Orange, 105, 5);
            AddBricksRow(Brick.BrickColor.Green, 130, 3);
            AddBricksRow(Brick.BrickColor.Green, 155, 3);
            AddBricksRow(Brick.BrickColor.Yellow, 180, 1);
            AddBricksRow(Brick.BrickColor.Yellow, 205, 1);
        }

        private void AddBricksRow(Brick.BrickColor rowColor, int height, int score)
        {
            var numberOfBricks = 14;
            var distanceBetweenBricks = (_gameCanvas.Width - numberOfBricks * Brick.Width)/numberOfBricks;
            double currentX = 1;
            for (var i = 0; i < numberOfBricks; i++)
            {
                var brick = new Brick(rowColor, score);
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
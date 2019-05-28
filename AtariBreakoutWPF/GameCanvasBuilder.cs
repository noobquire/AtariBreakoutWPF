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
            int startHeight = 70;
            int rowHeight = 25;
            AddBricksRow(Brick.BrickColor.Red, startHeight);
            AddBricksRow(Brick.BrickColor.Red, startHeight + rowHeight);
            AddBricksRow(Brick.BrickColor.Orange, startHeight + 2 * rowHeight);
            AddBricksRow(Brick.BrickColor.Orange, startHeight + 3* rowHeight);
            AddBricksRow(Brick.BrickColor.Green, startHeight + 4 * rowHeight);
            AddBricksRow(Brick.BrickColor.Green, startHeight + 5 * rowHeight);
            AddBricksRow(Brick.BrickColor.Yellow, startHeight + 6 * rowHeight);
            AddBricksRow(Brick.BrickColor.Yellow, startHeight + 7 * rowHeight);
        }

        private void AddBricksRow(Brick.BrickColor rowColor, int height)
        {
            var numberOfBricks = 14;
            var distanceBetweenBricks = (_gameCanvas.Width - numberOfBricks * Brick.Width)/numberOfBricks;
            double currentX = 1;
            for (var i = 0; i < numberOfBricks; i++)
            {
                var brick = new Brick(rowColor);
                SetPosition(brick.Shape, currentX, height);
                _gameCanvas.Canvas.Children.Add(brick.Shape);
                _gameCanvas.Bricks.Add(brick);
                currentX += distanceBetweenBricks + Brick.Width;
            }
        }

        public void AddBall()
        {
            _gameCanvas.AddBall();
        }

        public GameCanvas Build()
        {
            return _gameCanvas;
        }
    }
}
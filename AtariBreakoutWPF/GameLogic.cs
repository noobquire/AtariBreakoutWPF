using System;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AtariBreakoutWPF
{
    public sealed class GameLogic
    {
        public readonly GameCanvas GameCanvas;
        private readonly MovementLogic _movementLogic;
        //private readonly Timer _ballTimer;

        public GameLogic(Canvas gameCanvas)
        {
            

            var gameCanvasBuilder = new GameCanvasBuilder(gameCanvas);
            gameCanvasBuilder.AddPaddle();
            gameCanvasBuilder.AddBricks();
            gameCanvasBuilder.AddBall();
            GameCanvas = gameCanvasBuilder.Build();
            _movementLogic = new MovementLogic(GameCanvas);
        }

        public void Tick()
        {
            _movementLogic.MoveBall();
        }

        public void MovePaddle(Direction direction)
        {
            _movementLogic.MovePaddle(direction);
        }
    }
}
using System;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace AtariBreakoutWPF
{
    public sealed class GameLogic
    {
        /// <summary>
        /// The game field
        /// </summary>
        public readonly GameCanvas GameCanvas;
        /// <summary>
        /// Logic that controls movement of ball and paddle
        /// </summary>
        private readonly MovementLogic _movementLogic;

        public GameLogic(Canvas gameCanvas)
        {
            var gameCanvasBuilder = new GameCanvasBuilder(gameCanvas);
            gameCanvasBuilder.AddPaddle();
            gameCanvasBuilder.AddBricks();
            gameCanvasBuilder.AddBall();
            GameCanvas = gameCanvasBuilder.Build();
            _movementLogic = new MovementLogic(GameCanvas);
        }
        
        /// <summary>
        /// Represents one game tick.
        /// During a tick, ball moves for the least distance possible.
        /// This method must be called continuously by a ball timer. 
        /// </summary>
        public void Tick()
        {
            _movementLogic.MoveBall();
        }

        /// <summary>
        /// Moves paddle for the least possible distance.
        /// This method must be called continuously by a paddle timer.
        /// </summary>
        /// <param name="direction"></param>
        public void MovePaddle(Direction direction)
        {
            _movementLogic.MovePaddle(direction);
        }
    }
}
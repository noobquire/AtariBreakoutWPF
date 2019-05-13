using System.Windows.Controls;

namespace AtariBreakoutWPF
{
    public sealed class GameLogic
    {
        public readonly GameCanvas GameCanvas;
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

        public void SetBallSpeed(int newSpeed)
        {
            GameCanvas.Ball.Speed = newSpeed;
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
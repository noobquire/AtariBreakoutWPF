namespace AtariBreakoutWPF.CollisionDetection
{
    public class BallCollision
    {
        private GameCanvas _gameCanvas;
        public WallCollision OutBounds;
        public BrickCollision WithBrick;
        public PaddleCollision WithPaddle;

        public BallCollision(GameCanvas gameCanvas)
        {
            OutBounds = new WallCollision(gameCanvas);
            WithBrick = new BrickCollision(gameCanvas);
            WithPaddle = new PaddleCollision(gameCanvas);
            _gameCanvas = gameCanvas;
        }
    }
}
namespace AtariBreakoutWPF.CollisionDetection
{
    public class BallCollision
    {
        public WallCollision OutBounds;
        public BrickCollision WithBrick;
        public PaddleCollision WithPaddle;
         
        private GameCanvas _gameCanvas;

        public BallCollision(GameCanvas gameCanvas)
        {
            OutBounds = new WallCollision(gameCanvas);
            WithBrick = new BrickCollision(gameCanvas);
            WithPaddle = new PaddleCollision(gameCanvas);
            _gameCanvas = gameCanvas;
        }
    }
}
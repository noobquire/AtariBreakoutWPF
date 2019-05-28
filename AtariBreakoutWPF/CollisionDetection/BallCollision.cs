namespace AtariBreakoutWPF.CollisionDetection
{
    /// <summary>
    /// Represents logic required to detect collisions
    /// between the ball and obstacles on the game field.
    /// </summary>
    public class BallCollision
    {
        public WallCollision OutBounds;
        public BrickCollision WithBrick;
        public PaddleCollision WithPaddle;

        public BallCollision(GameCanvas gameCanvas)
        {
            OutBounds = new WallCollision(gameCanvas);
            WithBrick = new BrickCollision(gameCanvas);
            WithPaddle = new PaddleCollision(gameCanvas);
        }
    }
}
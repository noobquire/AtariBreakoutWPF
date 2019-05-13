using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AtariBreakoutWPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DispatcherTimer _ballTimer;
        private readonly GameLogic _logic;
        private readonly DispatcherTimer _paddleTimer;
        private Direction _direction = Direction.Default;

        public MainWindow()
        {
#if DEBUG // for exceptions in english
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
#endif
            InitializeComponent();

            _logic = new GameLogic(GameCanvas);

            _ballTimer = new DispatcherTimer(new TimeSpan(70000), DispatcherPriority.Background,
                (sender, args) => _logic.Tick(), Dispatcher);
            _paddleTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(0.7), DispatcherPriority.Background,
                (sender, args) => _logic.MovePaddle(_direction), Dispatcher);

            _ballTimer.Start();
            KeyDown += Game_OnKeyDown;
            KeyUp += Game_OnKeyUp;
            _logic.GameCanvas.ScoreChanged += Game_OnScoreChanged;
            _logic.GameCanvas.BallDestroyed += Game_OnBallDestroyed;
            _logic.GameCanvas.GameOver += Game_OnGameOver;
        }

        private void Game_OnGameOver(object sender, GameOverEventArgs e)
        {
            _paddleTimer.Stop();
            _ballTimer.Stop();
            KeyDown -= Game_OnKeyDown;
            KeyUp -= Game_OnKeyUp;
            MessageBox.Show($"Game over\r\nYour score: {e.FinalScore}", "Game over", MessageBoxButton.OK,
                MessageBoxImage.Exclamation);
        }

        private void Game_OnBallDestroyed(object sender, BallDestroyedEventArgs e)
        {
            BallCount.Text = $"x{e.NewBallCount}";
        }

        private void Game_OnScoreChanged(object sender, ScoreChangedEventArgs e)
        {
            Score.Text = $"Score: {e.NewScore}";
        }

        private void Game_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
                _direction = Direction.Left;
            else if (e.Key == Key.Right) _direction = Direction.Right;

            if (_direction != Direction.Default) _paddleTimer.Start();
        }

        private void Game_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right) _direction = Direction.Default;

            if (_direction != Direction.Default) _paddleTimer.Stop();
        }
    }
}
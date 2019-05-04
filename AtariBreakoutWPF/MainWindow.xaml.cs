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
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            InitializeComponent();

            _logic = new GameLogic(GameCanvas);

            _ballTimer = new DispatcherTimer(new TimeSpan(70000), DispatcherPriority.Background,
                (sender, args) => _logic.MoveBall(), Dispatcher);
            _paddleTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(0.7), DispatcherPriority.Background,
                (sender, args) => _logic.MovePaddle(_direction), Dispatcher);

            _ballTimer.Start();
            KeyDown += Game_OnKeyDown;
            KeyUp += Game_OnKeyUp;
            _logic.ScoreChanged += Game_OnScoreChanged;
            _logic.BallDestroyed += Game_OnBallDestroyed;
            _logic.GameOver += Game_OnGameOver;
            TestButton.Click += (s, e) => { _logic.SetBallSpeed(_logic.Ball.Speed != 0 ? 0 : 5); };
        }

        private void Game_OnGameOver(object sender, GameOverEventArgs e)
        {
            MessageBox.Show($"Game over\r\nYour score: {_logic.Score}", "Game over", MessageBoxButton.OK,
                MessageBoxImage.Exclamation);
            _paddleTimer.Stop();
            _ballTimer.Stop();
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
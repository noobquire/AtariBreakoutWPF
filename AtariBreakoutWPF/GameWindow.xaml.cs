using System;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.Security.AccessControl;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WPFCustomMessageBox;

namespace AtariBreakoutWPF
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GameWindow
    {
        private readonly DispatcherTimer _ballTimer;
        private readonly GameLogic _logic;
        private readonly DispatcherTimer _paddleTimer;
        private bool _paused;
        private Direction _direction = Direction.Default;

        public GameWindow()
        {
            InitializeComponent();

            _logic = new GameLogic(GameCanvas);

            _ballTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(0.7), DispatcherPriority.Background,
                (sender, args) => _logic.Tick(), Dispatcher);

            _paddleTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(0.5), DispatcherPriority.Background,
                (sender, args) => _logic.MovePaddle(_direction), Dispatcher);

            KeyDown += Game_OnKeyDown;
            KeyUp += Game_OnKeyUp;
            _logic.GameCanvas.ScoreChanged += Game_OnScoreChanged;
            _logic.GameCanvas.BallDestroyed += Game_OnBallDestroyed;
            _logic.GameCanvas.GameOver += Game_OnGameOver;
        }

        private void Game_OnGameOver(object sender, GameOverEventArgs e)
        {
            this.Deactivated -= Window_Deactivated;
            _paddleTimer.Stop();
            Pause();
            KeyDown -= Game_OnKeyDown;
            KeyUp -= Game_OnKeyUp;

            var exit =
                CustomMessageBox.ShowOKCancel($"Game over\r\nYour score: {e.FinalScore}", "Game over", "Play again", "Exit", MessageBoxImage.Exclamation);
            if (exit == MessageBoxResult.OK)
            {
                RestartGame();
            }
            else
            {
                Close();
            }
        }

        private void Pause()
        {
            _ballTimer.IsEnabled = false;
            _paddleTimer.IsEnabled = false;
            _paused = true;
        }

        private void Unpause()
        {
            _ballTimer.IsEnabled = true;
            _paddleTimer.IsEnabled = true;
            Application.Current.MainWindow = this;
            _paused = false;
        }

        private void RestartGame()
        {
            this.Closing -= Window_Closing;
            this.Deactivated -= Window_Deactivated;
            var form = new GameWindow();
            this.Hide();
            form.Show();
            Application.Current.MainWindow = form;
            this.Close();
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
            if (e.Key == Key.Right) _direction = Direction.Right;
            if (e.Key == Key.Escape && !_paused)
            {
                ShowPauseWindow();

            }

            if (_direction != Direction.Default) _paddleTimer.Start();
        }

        private void Game_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                _direction = Direction.Default;
            }
            else
            {
                _paddleTimer.Stop();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            ShowPauseWindow();
        }

        private void ShowPauseWindow()
        {
            this.Deactivated -= Window_Deactivated;
            Pause();
            var window = new PauseWindow();

            window.Owner = this;
            window.Top = this.Top + this.Height / 2 - window.Height / 2;
            window.Left = this.Left + this.Width / 2 - window.Width / 2;
            window.Closing += (s, e) => Unpause();


            var pause = window.ShowDialog();
            if (pause == PauseResult.Exit)
            {
                Application.Current.Shutdown();
            }

            if (pause == PauseResult.Continue)
            {
                Unpause();
                this.Deactivated += Window_Deactivated;
            }

            if (pause == PauseResult.Restart)
            {
                Unpause();
                RestartGame();
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowPauseWindow();
        }
    }
}
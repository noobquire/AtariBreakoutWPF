using System;
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
        private readonly DispatcherTimer _paddleTimer;
        private bool _paused;
        private Direction _direction = Direction.Default;

        public GameWindow()
        {
            InitializeComponent();

            var logic = new GameLogic(GameCanvas);

            _ballTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(0.7), DispatcherPriority.Background,
                (sender, args) => logic.Tick(), Dispatcher);

            _paddleTimer = new DispatcherTimer(TimeSpan.FromMilliseconds(0.5), DispatcherPriority.Background,
                (sender, args) => logic.MovePaddle(_direction), Dispatcher);

            KeyDown += Game_OnKeyDown;
            KeyUp += Game_OnKeyUp;
            logic.GameCanvas.ScoreChanged += Game_OnScoreChanged;
            logic.GameCanvas.BallDestroyed += Game_OnBallDestroyed;
            logic.GameCanvas.GameOver += Game_OnGameOver;
        }

        private void Game_OnGameOver(object sender, GameOverEventArgs e)
        {
            Deactivated -= Window_Deactivated;
            _paddleTimer.Stop();
            Pause();
            KeyDown -= Game_OnKeyDown;
            KeyUp -= Game_OnKeyUp;

            if (e.Reason == GameOverReason.Lost)
            {
                var exit =
                                CustomMessageBox.ShowOKCancel($"You lost your last ball\r\nYour score: {e.FinalScore}", "Game over", "Play again", "Exit", MessageBoxImage.Exclamation);
                            if (exit == MessageBoxResult.OK)
                            {
                                RestartGame();
                            }
                            else
                            {
                                Close();
                            }
            } else if (e.Reason == GameOverReason.Won)
            {
                var exit =
                    CustomMessageBox.ShowOKCancel($"\r\nCongratulations, you won!\r\nYour score: {e.FinalScore}", "Game finished", "Play again", "Exit", MessageBoxImage.Exclamation);
                if (exit == MessageBoxResult.OK)
                {
                    RestartGame();
                }
                else
                {
                    Close();
                }
            }
            
        }

        private void Pause()
        {
            _ballTimer.IsEnabled = false;
            KeyDown -= Game_OnKeyDown;
            _paused = true;
        }

        private void Unpause()
        {
            _ballTimer.IsEnabled = true;
            KeyDown += Game_OnKeyDown;
            Application.Current.MainWindow = this;
            _paused = false;
        }

        private void RestartGame()
        {
            Closing -= Window_Closing;
            Deactivated -= Window_Deactivated;
            var form = new GameWindow();
            Hide();
            form.Show();
            Application.Current.MainWindow = form;
            Close();
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
            Deactivated -= Window_Deactivated;
            Pause();
            var window = new PauseWindow();

            window.Owner = this;
            window.Top = Top + Height / 2 - window.Height / 2;
            window.Left = Left + Width / 2 - window.Width / 2;
            window.Closing += (s, e) => Unpause();


            var pause = window.ShowDialog();
            if (pause == PauseResult.Exit)
            {
                Application.Current.Shutdown();
            }

            if (pause == PauseResult.Continue)
            {
                Unpause();
                Deactivated += Window_Deactivated;
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
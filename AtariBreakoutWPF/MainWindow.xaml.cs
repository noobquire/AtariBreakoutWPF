using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace AtariBreakoutWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Direction direction = Direction.Default;
        private GameLogic logic;
        private DispatcherTimer paddleTimer;
        public MainWindow()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            InitializeComponent();
            
            paddleTimer = new DispatcherTimer(new TimeSpan(200000), DispatcherPriority.Background, (sender, args) => logic.MovePaddle(direction), Dispatcher);
            logic = new GameLogic(GameCanvas);
            DispatcherTimer ballTimer = new DispatcherTimer(new TimeSpan(70000), DispatcherPriority.Background, (sender, args) => logic.MoveBall(), Dispatcher);
            ballTimer.Start();
            KeyDown += Game_OnKeyDown;
            KeyUp += Game_OnKeyUp;
            TestButton.Click += (s, e) =>
            {
                if (logic.ball.Speed != 0)
                {
                    logic.SetBallSpeed(0);
                }
                else
                {
                    logic.SetBallSpeed(5);
                }
            };
        }
        
        private void Game_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            { 
                direction = Direction.Left;
            } else if (e.Key == Key.Right)
            {
                direction = Direction.Right;
            }

            if (direction != Direction.Default)
            {
                paddleTimer.Start();
            }
        }

        private void Game_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left || e.Key == Key.Right)
            {
                direction = Direction.Default;
            }

            if (direction != Direction.Default)
            {
                paddleTimer.Stop();
            }
        }
        
    }
}

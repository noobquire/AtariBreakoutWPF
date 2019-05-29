using System;
using System.Windows;
using Gat.Controls;

namespace AtariBreakoutWPF
{
    /// <summary>
    /// Interaction logic for PauseWindow.xaml
    /// </summary>
    public partial class PauseWindow : Window
    {
        private PauseResult _pauseResult;

        public PauseWindow()
        {
            InitializeComponent();
        }

        public new PauseResult ShowDialog()
        {
            base.ShowDialog();
            return _pauseResult;
        }
        

        private void ContinueButton_OnClick(object sender, RoutedEventArgs e)
        {
            _pauseResult = PauseResult.Continue;
            DialogResult = true;
        }

        private void RestartButton_OnClick(object sender, RoutedEventArgs e)
        {
            _pauseResult = PauseResult.Restart;
            DialogResult = true;
        }

        private void ExitButton_OnClick(object sender, RoutedEventArgs e)
        {
            var messageBox = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (messageBox == MessageBoxResult.Yes)
            {
                _pauseResult = PauseResult.Exit;
                DialogResult = true;
            }
        }

        private void AboutButton_OnClick(object sender, RoutedEventArgs e)
        {
            var aboutBox = new About();
            aboutBox.AdditionalNotes = "";
            aboutBox.Show();
        }
    }

    public enum PauseResult
    {
        Continue,
        Restart,
        Exit
    }
}

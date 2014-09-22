using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Mp3Player
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            _mainViewModel = (MainViewModel)DataContext;
            _mainViewModel.PlayRequested += (sender, e) => MediaPlayer.Play();
            _mainViewModel.PauseRequested += (sender, e) => MediaPlayer.Pause();
            _mainViewModel.StopRequested += (sender, e) => MediaPlayer.Stop();
        }

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            _mainViewModel.MediaLength = (int)MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void TrackSlider_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            MediaPlayer.Position = new TimeSpan(0, 0, 0, (int)TrackSlider.Value, 0);
        }
    }
}
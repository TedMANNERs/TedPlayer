﻿using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

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
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Style style = null;
            if (SystemParameters.IsGlassEnabled)
            {
                style = (Style)Resources["WindowStyle"];
            }
            Style = style;
        }

        private void MediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            _mainViewModel.MediaLength = (int)MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
        }

        private void TrackSlider_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            MediaPlayer.Position = new TimeSpan(0, 0, 0, (int)TrackSlider.Value, 0);
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!VolumeControl.IsMouseOver)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void WindowStateButton_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
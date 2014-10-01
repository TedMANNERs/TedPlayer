using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;
using Microsoft.Win32;
using Mp3Player.Annotations;

namespace Mp3Player
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private Uri _currentTrack;
        private bool _isPlaying;
        private int _mediaLength;
        private int _mediaPosition;
        private ObservableCollection<Uri> _trackList = new ObservableCollection<Uri>();
        private float _volume = 1f;

        public MainViewModel()
        {
            PlayPauseCommand = new DelegateCommand(obj => PlayPause(), () => TrackList.Any());
            StopCommand = new DelegateCommand(obj => Stop(), () => TrackList.Any());
            OpenCommand = new DelegateCommand(obj => Open(), () => true);
            PreviousCommand = new DelegateCommand(obj => Previous(), () => TrackList.Any());
            NextCommand = new DelegateCommand(obj => Next(), () => TrackList.Any());
            SliderTimer = new DispatcherTimer();
            SliderTimer.Tick += OnTimedEvent;
            SliderTimer.Interval = TimeSpan.FromMilliseconds(1000);
        }

        public ICommand PlayPauseCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }
        public DispatcherTimer SliderTimer { get; set; }

        public ObservableCollection<Uri> TrackList
        {
            get { return _trackList; }
            set
            {
                _trackList = value;
                OnPropertyChanged();
            }
        }

        public Uri CurrentTrack
        {
            get { return _currentTrack; }
            set
            {
                _currentTrack = value;
                OnPropertyChanged();
            }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }

        public float Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyChanged();
            }
        }

        public int MediaPosition
        {
            get { return _mediaPosition; }
            set
            {
                _mediaPosition = value;
                OnPropertyChanged();
            }
        }

        public int MediaLength
        {
            get { return _mediaLength; }
            set
            {
                _mediaLength = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void PlayPause()
        {
            if (IsPlaying)
            {
                PauseRequested(this, EventArgs.Empty);
                SliderTimer.IsEnabled = false;
                IsPlaying = false;
            }
            else
            {
                PlayRequested(this, EventArgs.Empty);
                SliderTimer.Start();
                IsPlaying = true;
            }
        }

        private void Stop()
        {
            StopRequested(this, EventArgs.Empty);
            SliderTimer.Stop();
            IsPlaying = false;
            MediaPosition = 0;
        }

        private void Open()
        {
            OpenFileDialog fdialog = new OpenFileDialog { DefaultExt = ".mp3", Filter = "Audio Files (*.mp3,*.wav,*.wmv )|*.mp3;*.wav;*.wmv", Multiselect = true };
            bool? result = fdialog.ShowDialog();
            if (!result.HasValue || !(bool)result)
                return;
            foreach (string fileName in fdialog.FileNames)
            {
                TrackList.Add(new Uri(fileName));
            }
            CurrentTrack = TrackList.First();
        }

        private void Previous()
        {
            if (TrackList.IndexOf(CurrentTrack) > 0)
                CurrentTrack = TrackList.ElementAt(TrackList.IndexOf(CurrentTrack) - 1);
            else
                CurrentTrack = TrackList.Last();
            Stop();
            PlayPause();
        }

        private void Next()
        {
            if (TrackList.IndexOf(CurrentTrack) < TrackList.Count - 1)
                CurrentTrack = TrackList.ElementAt(TrackList.IndexOf(CurrentTrack) + 1);
            else
                CurrentTrack = TrackList.First();
            Stop();
            PlayPause();
        }

        public void OnTimedEvent(object sender, EventArgs eventArgs)
        {
            if (MediaPosition < MediaLength)
            {
                MediaPosition++;
            }
            else
            {
                SliderTimer.Stop();
            }
        }

        public event EventHandler PlayRequested;
        public event EventHandler PauseRequested;
        public event EventHandler StopRequested;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
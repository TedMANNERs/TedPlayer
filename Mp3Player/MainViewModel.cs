using Microsoft.Win32;
using Mp3Player.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Mp3Player
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Uri> trackList = new ObservableCollection<Uri>();
        private Uri currentTrack;
        private bool isPlaying;
        private float volume = 1f;
        private int timeSlidervalue;
        private Timer sliderTimer = new Timer();

        public MainViewModel()
        {
            PlayPauseCommand = new DelegateCommand(obj => PlayPause(), () => TrackList.Any());
            StopCommand = new DelegateCommand(obj => Stop(), () => TrackList.Any());
            OpenCommand = new DelegateCommand(obj => Open(), () => true);
            PreviousCommand = new DelegateCommand(obj => Previous(), () => TrackList.Any());
            NextCommand = new DelegateCommand(obj => Next(), () => TrackList.Any());
            sliderTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            sliderTimer.Interval = 1000;
        }

        public ICommand PlayPauseCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand PreviousCommand { get; private set; }
        public ICommand NextCommand { get; private set; }

        public ObservableCollection<Uri> TrackList
        {
            get { return trackList; }
            set 
            { 
                trackList = value; 
                OnPropertyChanged();
            }
        }

        public Uri CurrentTrack
        {
            get { return currentTrack; }
            set 
            { 
                currentTrack = value;
                OnPropertyChanged();
            }
        }

        public bool IsPlaying
        {
            get { return isPlaying; }
            set
            { 
                isPlaying = value; 
                OnPropertyChanged();
            }
        }

        public float Volume
        {
            get { return volume; }
            set 
            { 
                volume = value;
                OnPropertyChanged();
            }
        }

        public int TimeSliderValue
        {
            get { return timeSlidervalue; }
            set 
            { 
                timeSlidervalue = value;
                OnPropertyChanged();
            }
        }
        
        private void PlayPause()
        {
            if (IsPlaying)
            {
                PauseRequested(this, EventArgs.Empty);
                sliderTimer.Enabled = false;
                IsPlaying = false;
            }
            else
            {
                PlayRequested(this, EventArgs.Empty);
                sliderTimer.Start();
                IsPlaying = true;
            }
        }

        private void Stop()
        {
            StopRequested(this, EventArgs.Empty);
            sliderTimer.Stop();
            IsPlaying = false;
        }

        private void Open()
        {
            OpenFileDialog fdialog = new OpenFileDialog();
            fdialog.DefaultExt = ".mp3";
            fdialog.Filter = "Audio Files (*.mp3,*.wav,*.wmv )|*.mp3;*.wav;*.wmv";
            fdialog.Multiselect = true;
            bool? result = fdialog.ShowDialog();
            if (result == true)
            {
                foreach (var fileName in fdialog.FileNames)
                {
                    TrackList.Add(new Uri(fileName));
                }
                CurrentTrack = TrackList.First();
            }
        }

        private void Previous()
        {
            if (TrackList.IndexOf(CurrentTrack) > 0)
                CurrentTrack = TrackList.ElementAt(TrackList.IndexOf(CurrentTrack) - 1);
            else
                CurrentTrack = TrackList.Last();
        }

        private void Next()
        {
            if (TrackList.IndexOf(CurrentTrack) < TrackList.Count - 1)
                CurrentTrack = TrackList.ElementAt(TrackList.IndexOf(CurrentTrack) + 1);
            else
                CurrentTrack = TrackList.First();
        }

        private void IncrementTimerValue()
        {
            TimeSliderValue++;
        }

        public event EventHandler PlayRequested;
        private void OnPlayRequested(object sender, EventArgs e)
        {
            if (PlayRequested != null)
                PlayRequested(this, EventArgs.Empty);
        }

        public event EventHandler PauseRequested;
        private void OnPauseRequested(object sender, EventArgs e)
        {
            if (PauseRequested != null)
                PauseRequested(this, EventArgs.Empty);
        }

        public event EventHandler StopRequested;
        private void OnStopRequested(object sender, EventArgs e)
        {
            if (StopRequested != null)
                StopRequested(this, EventArgs.Empty);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(IncrementTimerValue);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

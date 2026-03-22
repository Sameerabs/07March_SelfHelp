using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;


namespace SelfHelp
{
    public partial class PracticePlayerPage : ContentPage
    {
        private Stopwatch _globalStopwatch = new Stopwatch();
        public string Date { get; set; }
        public string Time { get; set; }
        //  private IAudioPlayer _player;
        private bool _isStopping = false;
        private int _elapsedTime = 0;
        private double _totalTime = 0;
        private List<AudioSequence> _sequence;
        private int _currentStep = 0;
        private string _playlistName;
        private int _userPracticeTime;
        private string _currentPracticeName;
        private double _currentItemTotalTime;

        public PracticePlayerPage(string playlistName, int userInputMinutes)
        {
            InitializeComponent();
            _playlistName = playlistName;

            PlaylistTitle.Text = _playlistName;

            _sequence = PracticeAudioMap.GetPracticeSequence(playlistName);

            _totalTime = CalculateTotalPracticeTime(_sequence, userInputMinutes);
            TotalTimeLabel.Text = $"Total Time: {TimeSpan.FromSeconds(_totalTime):hh\\:mm\\:ss}";
            _userPracticeTime = userInputMinutes * 60; // Convert minutes to seconds

        }

        public PracticePlayerPage(string playlistName, List<AudioSequence> sequence)
        {
            InitializeComponent();
            _sequence = sequence;
            _playlistName = playlistName;

            PlaylistTitle.Text = _playlistName;
         //   _totalTime = _sequence.Sum(s => s.LoopDuration); // Calculate total time
            _totalTime = CalculateTotalPracticeTime(_sequence, 0);

           // TotalTimeLabel.Text = $"Total Time: {TimeSpan.FromSeconds(_totalTime):mm\\:ss}";
            TotalTimeLabel.Text = $"Total Time: {TimeSpan.FromSeconds(_totalTime):hh\\:mm\\:ss}";
        }

        private double CalculateTotalPracticeTime(List<AudioSequence> sequence, int userInputDuration)
        {
            double totalTime = 0;

            foreach (var step in sequence)
            {
                if (step.PlayerLoop)
                {
                    if (step.LoopDuration == -1)
                    {
                        // 🔹 Only use user input duration if it's a user-controlled practice
                        totalTime += userInputDuration * 60; // Convert minutes to seconds
                    }
                    else
                    {
                        totalTime += step.LoopDuration; // 🔹 Use predefined loop duration
                    }
                }
                else
                {
                    // 🔹 Non-looping files should only add their real duration
                    totalTime += AudioMetadata.GetAudioLength(step.FileName);
                }
            }

            return totalTime;
        }



        private async void OnPlayClicked(object sender, EventArgs e)
        {
            _globalStopwatch.Restart();
            AudioPlayerService.Instance.Stop();
            _isStopping = false;

            // 🔹 FIX: Reset BEFORE starting
            _currentStep = 0;
            _elapsedTime = 0;
            ProgressSlider.Value = 0;
            ElapsedTimeLabel.Text = "Elapsed Time: 00:00:00";

            PlayButton.IsEnabled = false;
            StopButton.IsEnabled = true;

            await PlayPracticeSequence();

            if (!_isStopping) // Only if user didn't stop manually
            {
                var session = new PracticeSession
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Time = DateTime.Now.ToString("HH:mm"),
                    PracticeName = _playlistName,
                    DurationMinutes = (int)Math.Round(_globalStopwatch.Elapsed.TotalMinutes),
                    Completed = true
                };

                await PracticeStatsService.AddSessionAsync(session);
            }

            // 🔹 After sequence finishes
            PlayButton.IsEnabled = true;
            StopButton.IsEnabled = false;

            _currentStep = 0;
            _elapsedTime = 0;
            ProgressSlider.Value = 0;
            ElapsedTimeLabel.Text = "Elapsed Time: 00:00:00";
        }

        private async Task PlayPracticeSequence()
        {
            for (; _currentStep < _sequence.Count; _currentStep++)
            {
                if (_isStopping) return;

                var step = _sequence[_currentStep];

                string audioName = step.FileName
                        .Replace(".mp3", "")
                        .Replace("_", " ");

                _currentPracticeName = step.PracticeName;

                // 🔹 Set practice name
                PracticeTitle.Text = _currentPracticeName;

                // 🔹 Optional: show only step/audio
                NowPlayingLabel.Text = audioName;


                //NowPlayingLabel.Text = step.FileName;

                double loopTime;

                if (step.PlayerLoop && step.LoopDuration == -1)
                {
                    loopTime = _userPracticeTime; // 🔹 Use user input duration, not _totalTime
                   // loopTime = _totalTime - _elapsedTime; // Use user input duration
                }
                else if (step.LoopDuration == 0)
                {
                    loopTime = AudioMetadata.GetAudioLength(step.FileName); // Ensure it plays once
                }
                else 
                {
                    loopTime = step.LoopDuration; // Use predefined dictionary duration
                }

                _currentItemTotalTime = loopTime;
                await PlayAudio(step.FileName, loop: step.PlayerLoop, duration: loopTime);



                CurrentItemTotalLabel.Text =
                    $"Current Item Total: {TimeSpan.FromSeconds(loopTime):hh\\:mm\\:ss}";
            }
        }


        private async Task PlayAudio(string fileName, bool loop, double duration)
        {
            try
            {
                int tempTime = 0;

                // 🔹 Load audio file
                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                if (stream == null)
                {
                    await DisplayAlert("Error", $"Audio file not found: {fileName}", "OK");
                    return;
                }

                // 🔹 Play using global service
                await AudioPlayerService.Instance.PlayAsync(stream, loop);





                var stopwatch = Stopwatch.StartNew();

                while (AudioPlayerService.Instance.IsPlaying && !_isStopping)
                {
                    await Task.Delay(100); // 🔥 100ms precision

                    double elapsed = stopwatch.Elapsed.TotalSeconds;

                    tempTime = (int)elapsed;
                    _elapsedTime = (int)_globalStopwatch.Elapsed.TotalSeconds;

                    ElapsedTimeLabel.Text = $"Elapsed Time: {TimeSpan.FromSeconds(_elapsedTime):hh\\:mm\\:ss}";
                    ProgressSlider.Value = _globalStopwatch.Elapsed.TotalSeconds / _totalTime;

                    CurrentItemElapsedLabel.Text = 
                        $"Current Item Elapsed: {TimeSpan.FromSeconds(elapsed):hh\\:mm\\:ss}";

                    if (elapsed >= duration && duration > 0)
                    {
                        AudioPlayerService.Instance.Stop();
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Unable to play audio: {ex.Message}", "OK");
            }
        }


        private void OnPauseClicked(object sender, EventArgs e)
        {
            _isStopping = true;
            AudioPlayerService.Instance.Pause();
        }

        private async void OnStopClicked(object sender, EventArgs e)
        {
            _isStopping = true;

            if (_globalStopwatch.Elapsed.TotalSeconds > 5) // avoid tiny sessions
            {
                var session = new PracticeSession
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Time = DateTime.Now.ToString("HH:mm"),
                    PracticeName = _playlistName,
                    DurationMinutes = (int)Math.Round(_globalStopwatch.Elapsed.TotalMinutes),
                    Completed = false
                };

                await PracticeStatsService.AddSessionAsync(session);
            }


            AudioPlayerService.Instance.Stop();

            _elapsedTime = 0;
            _currentStep = 0;

            ProgressSlider.Value = 0;

            ElapsedTimeLabel.Text = "Elapsed Time: 00:00:00";
            CurrentItemElapsedLabel.Text = "Current Item Elapsed: 00:00";

            PlayButton.IsEnabled = true;
            StopButton.IsEnabled = false;


        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // 🔴 Stop audio when leaving page
            AudioPlayerService.Instance.Stop();
        }
    }

}

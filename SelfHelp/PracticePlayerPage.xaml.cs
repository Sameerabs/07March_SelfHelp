using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SelfHelp
{
    public partial class PracticePlayerPage : ContentPage
    {
        private IAudioPlayer _player;
        private bool _isStopping = false;
        private int _elapsedTime = 0;
        private int _totalTime = 0;
        private List<AudioSequence> _sequence;
        private int _currentStep = 0;
        private string _practiceName;
        private int _userPracticeTime;

        public PracticePlayerPage(string practiceName, int userInputMinutes)
        {
            InitializeComponent();
            _practiceName = practiceName;
          //  _totalTime = userInputMinutes * 60; // Convert to seconds

            PracticeTitle.Text = practiceName;

            _sequence = PracticeAudioMap.GetPracticeSequence(practiceName);

            _totalTime = CalculateTotalPracticeTime(_sequence, userInputMinutes);
            TotalTimeLabel.Text = $"Total Time: {TimeSpan.FromSeconds(_totalTime):hh\\:mm\\:ss}";
            //TotalTimeLabel.Text = $"Total Time: {TimeSpan.FromSeconds(_totalTime):mm\\:ss}";
            _userPracticeTime = userInputMinutes * 60; // Convert minutes to seconds

        }

        public PracticePlayerPage(string practiceName, List<AudioSequence> sequence)
        {
            InitializeComponent();
            _sequence = sequence;

            PracticeTitle.Text = practiceName;
         //   _totalTime = _sequence.Sum(s => s.LoopDuration); // Calculate total time
            _totalTime = CalculateTotalPracticeTime(_sequence, 0);

           // TotalTimeLabel.Text = $"Total Time: {TimeSpan.FromSeconds(_totalTime):mm\\:ss}";
            TotalTimeLabel.Text = $"Total Time: {TimeSpan.FromSeconds(_totalTime):hh\\:mm\\:ss}";
        }

        private int CalculateTotalPracticeTime(List<AudioSequence> sequence, int userInputDuration)
        {
            int totalTime = 0;

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
            _isStopping = false;
            await PlayPracticeSequence();
        }

        private async Task PlayPracticeSequence()
        {
            for (; _currentStep < _sequence.Count; _currentStep++)
            {
                if (_isStopping) return;

                var step = _sequence[_currentStep];

                int loopTime;

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

                await PlayAudio(step.FileName, loop: step.PlayerLoop, duration: loopTime);
            }
        }


        private async Task PlayAudio(string fileName, bool loop, int duration)
        {
            try
            {
                int tempTime = 0;
                _player?.Stop();
                _player?.Dispose();

                // 🔹 Check if the file exists in the app package
                using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
                if (stream == null)
                {
                    await DisplayAlert("Error", $"Audio file not found: {fileName}", "OK");
                    return;
                }

                // 🔹 Create and configure the audio player
                _player = AudioManager.Current.CreatePlayer(stream);
                _player.Loop = loop;
                _player.Play();

                if (!_player.IsPlaying)
                {
                    await DisplayAlert("Error", "Audio failed to play. Ensure file is in Resources/Raw", "OK");
                    return;
                }

                int elapsed = 0;
                while (_player.IsPlaying && !_isStopping) // && elapsed < duration)
                {
                    await Task.Delay(1000);
                    elapsed++;
                    tempTime++;
                    _elapsedTime++;



                    ElapsedTimeLabel.Text = $"Elapsed Time: {TimeSpan.FromSeconds(_elapsedTime):hh\\:mm\\:ss}";
                    TempTimeLabel.Text = $"Temp Time: {TimeSpan.FromSeconds(tempTime):hh\\:mm\\:ss}";

                  //  TotalTimeLabel.Text = $"Total Time: {TimeSpan.FromSeconds(_totalTime):hh\\:mm\\:ss}";

                    ProgressSlider.Value = (double)_elapsedTime / _totalTime;

                    if (elapsed == duration && duration >0)
                    {
                        _player.Loop = false;
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
            _player?.Pause();
        }

        private void OnStopClicked(object sender, EventArgs e)
        {
            _isStopping = true;
            _player?.Stop();
            _elapsedTime = 0;
            _currentStep = 0;
            ProgressSlider.Value = 0;
            ElapsedTimeLabel.Text = "Elapsed Time: 0:00";
        }


    }
}

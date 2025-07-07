using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SelfHelp
{
    public partial class NewTimerPage : ContentPage
    {
        private JsonStorageService _storageService;
        private List<TimerPreset> _timers = new List<TimerPreset>();
        private int _selectedIndex = -1; // Track the selected timer index

        public NewTimerPage()
        {
            InitializeComponent();
            _storageService = new JsonStorageService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadTimersAsync();
        }

        private void OnPresetSelected(int index)
        {
            Console.WriteLine($"Selected Index: {index} / Total Items: {_timers.Count}"); // ✅ Debugging log

            if (index < 0 || index >= _timers.Count) return;

            _selectedIndex = index; // ✅ Assign correct index

            // 🔹 Refresh UI highlight without full reload
            for (int i = 0; i < TimerStackLayout.Children.Count; i++)
            {
                if (TimerStackLayout.Children[i] is Frame frame)
                {
                    frame.BorderColor = (i == _selectedIndex) ? Colors.Blue : Colors.Gray;
                    frame.BackgroundColor = (i == _selectedIndex) ? Colors.LightBlue : Colors.Transparent;
                }
            }
        }

        private List<AudioSequence> ConvertPresetToAudioSequence(TimerPreset preset)
        {
            List<AudioSequence> sequence = new List<AudioSequence>();

            for (int i = 0; i < preset.TotalIntervals; i++)
            {
                sequence.Add(new AudioSequence("IntervalBeep.mp3", preset.IntervalDuration, true));

                if (i < preset.TotalIntervals - 1) // Add gap if not the last interval
                {
                    sequence.Add(new AudioSequence("Silence.mp3", preset.GapBetweenIntervals, true));
                }
            }

            sequence.Add(new AudioSequence("EndChime.mp3", 0, false)); // Final chime
            return sequence;
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            string timerName = TimerNameEntry.Text?.Trim();
            if (string.IsNullOrEmpty(timerName))
            {
                await DisplayAlert("Error", "Please enter a name for the timer.", "OK");
                return;
            }

            if (!int.TryParse(IntervalHours.Text, out int intervalH)) intervalH = 0;
            if (!int.TryParse(IntervalMinutes.Text, out int intervalM)) intervalM = 0;
            if (!int.TryParse(IntervalSeconds.Text, out int intervalS)) intervalS = 0;

            if (!int.TryParse(TotalIntervals.Text, out int totalIntervals) || totalIntervals < 1 || totalIntervals > 99)
            {
                await DisplayAlert("Error", "Total Intervals must be between 1 and 99.", "OK");
                return;
            }

            if (!int.TryParse(GapHours.Text, out int gapH)) gapH = 0;
            if (!int.TryParse(GapMinutes.Text, out int gapM)) gapM = 0;
            if (!int.TryParse(GapSeconds.Text, out int gapS)) gapS = 0;

            var newPreset = new TimerPreset
            {
                Name = timerName,
                IntervalDuration = (intervalH * 3600) + (intervalM * 60) + intervalS,
                TotalIntervals = totalIntervals,
                GapBetweenIntervals = (gapH * 3600) + (gapM * 60) + gapS
            };

            _timers.Add(newPreset);
            await _storageService.SaveTimersAsync(_timers);
            await LoadTimersAsync(); // Refresh list after saving
        }

        private async Task LoadTimersAsync()
        {
            TimerStackLayout.Children.Clear();
            _timers = await _storageService.LoadTimersAsync();

            if (_timers.Count == 0)
            {
                PlayAllButton.IsVisible = false;
                return;
            }

            for (int i = 0; i < _timers.Count; i++)
            {
                var timer = _timers[i]; // Get correct timer from list
                int currentIndex = i; // ✅ Store correct index in a local variable

                var timerFrame = new Frame
                {
                    Padding = 10,
                    CornerRadius = 10,
                    BorderColor = (_selectedIndex == currentIndex) ? Colors.Blue : Colors.Gray, // ✅ Preserve selection
                    BackgroundColor = (_selectedIndex == currentIndex) ? Colors.LightBlue : Colors.Transparent, // ✅ Keep highlight
                    Content = new VerticalStackLayout
                    {
                        Spacing = 5,
                        Children =
                {
                    new Label { Text = $"📌 {timer.Name}", FontSize = 18, FontAttributes = FontAttributes.Bold },
                    new Label { Text = $"⏳ Interval Duration: {TimeSpan.FromSeconds(timer.IntervalDuration):hh\\:mm\\:ss}" },
                    new Label { Text = $"🔁 Total Intervals: {timer.TotalIntervals}" },
                    new Label { Text = $"⏸ Gap Between: {TimeSpan.FromSeconds(timer.GapBetweenIntervals):hh\\:mm\\:ss}" }
                }
                    }
                };

                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) => OnPresetSelected(currentIndex); // ✅ Use the local variable
                timerFrame.GestureRecognizers.Add(tapGesture);

                TimerStackLayout.Children.Add(timerFrame);
            }

            PlayAllButton.IsVisible = true;
        }

        private void OnTotalIntervalsChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(TotalIntervals.Text, out int totalIntervals) && totalIntervals > 1)
            {
                // 🔹 Enable the gap duration fields if total intervals > 1
                GapHours.IsEnabled = true;
                GapMinutes.IsEnabled = true;
                GapSeconds.IsEnabled = true;
            }
            else
            {
                // 🔹 Disable gap fields if only 1 interval is selected
                GapHours.IsEnabled = false;
                GapMinutes.IsEnabled = false;
                GapSeconds.IsEnabled = false;
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (_selectedIndex < 0 || _selectedIndex >= _timers.Count)
            {
                await DisplayAlert("Error", "Please select a timer to delete.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Delete Timer", $"Are you sure you want to delete '{_timers[_selectedIndex].Name}'?", "Yes", "No");
            if (!confirm) return;

            _timers.RemoveAt(_selectedIndex);
            _selectedIndex = -1; // Reset selection
            await _storageService.SaveTimersAsync(_timers);
            await LoadTimersAsync();
        }

        private async void OnMoveUpClicked(object sender, EventArgs e)
        {
            if (_selectedIndex <= 0)
            {
                await DisplayAlert("Error", "Cannot move this timer up.", "OK");
                return;
            }

            var temp = _timers[_selectedIndex];
            _timers[_selectedIndex] = _timers[_selectedIndex - 1];
            _timers[_selectedIndex - 1] = temp;

            _selectedIndex--; // Move selection up
            await _storageService.SaveTimersAsync(_timers);
            await LoadTimersAsync();
        }
        private async void OnMoveDownClicked(object sender, EventArgs e)
        {
            if (_selectedIndex < 0 || _selectedIndex >= _timers.Count - 1)
            {
                await DisplayAlert("Error", "Cannot move this timer down.", "OK");
                return;
            }

            var temp = _timers[_selectedIndex];
            _timers[_selectedIndex] = _timers[_selectedIndex + 1];
            _timers[_selectedIndex + 1] = temp;

            _selectedIndex++; // Move selection down
            await _storageService.SaveTimersAsync(_timers);
            await LoadTimersAsync();
        }


        private List<AudioSequence> ConvertPresetsToAudioSequences(List<TimerPreset> presets)
        {
            List<AudioSequence> fullSequence = new List<AudioSequence>();

            // 🔹 Start with 5-sec silence and transition bell
            fullSequence.Add(new AudioSequence("1Sec_Silence.mp3", loopDuration: 5, playerLoop: true));
            fullSequence.Add(new AudioSequence("Bell_Transition.mp3", loopDuration: 0, playerLoop: false));

            for (int i = 0; i < presets.Count; i++)
            {
                var preset = presets[i];

                for (int j = 0; j < preset.TotalIntervals; j++)
                {
                    // 🔹 Interval Duration (User-defined)
                    fullSequence.Add(new AudioSequence("1Sec_Silence.mp3", loopDuration: preset.IntervalDuration, playerLoop: true));

                    // ✅ Only play `Bell1.mp3` **if this is NOT the last interval**
                    if (j < preset.TotalIntervals - 1)
                    {
                        fullSequence.Add(new AudioSequence("Bell1.mp3", loopDuration: 0, playerLoop: false));
                    }

                    // 🔹 Add gap if there are multiple intervals
                    if (j < preset.TotalIntervals - 1 && preset.GapBetweenIntervals >0)
                    {
                        fullSequence.Add(new AudioSequence("small_Bell_Sound.mp3", loopDuration: 0, playerLoop: false));
                        fullSequence.Add(new AudioSequence("1Sec_Silence.mp3", loopDuration: preset.GapBetweenIntervals, playerLoop: true));
                        fullSequence.Add(new AudioSequence("small_Bell_Sound.mp3", loopDuration: 0, playerLoop: false));
                    }
                }

                // ✅ At the end of a preset, choose between `2Bells.mp3` and `3Bells.mp3`
                if (i < presets.Count - 1)
                {
                    // 🔹 If more presets exist, transition with 2 bells
                    fullSequence.Add(new AudioSequence("2Bells.mp3", loopDuration: 0, playerLoop: false));
                }
                else
                {
                    // 🔹 If this is the last preset, end with 3 bells
                    fullSequence.Add(new AudioSequence("3Bells.mp3", loopDuration: 0, playerLoop: false));
                }
            }

            return fullSequence;
        }
        private async void OnPlayAllClicked(object sender, EventArgs e)
        {
            if (_timers.Count == 0)
            {
                await DisplayAlert("Error", "No timers available!", "OK");
                return;
            }

            // 🔹 Convert all timers to AudioSequences
            List<AudioSequence> allSequences = ConvertPresetsToAudioSequences(_timers);

            // 🔹 Navigate to PracticePlayerPage with the full sequence
            await Navigation.PushAsync(new PracticePlayerPage("All Timers", allSequences));
        }



    }
}

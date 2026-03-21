using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SelfHelp
{
    public partial class YogaPlaylistPage : ContentPage
    {
        private List<PlaylistItem> _playlist = new List<PlaylistItem>();
        private int _selectedIndex = -1; // Track selected practice

        public YogaPlaylistPage()
        {
            InitializeComponent();
            LoadPlaylist();
        }

        private async void LoadPlaylist()
        {
            PlaylistStackLayout.Children.Clear(); // Clear previous items
            _playlist = await YogaPlaylistService.LoadPlaylistAsync();

            if (_playlist.Count == 0)
            {
                PlayAllButton.IsEnabled = false;
                return;
            }

            // Populate UI dynamically
            for (int i = 0; i < _playlist.Count; i++)
            {
                var practice = _playlist[i];
                int currentIndex = i; // Capture index for tap gesture

                var practiceFrame = new Frame
                {
                    Padding = 10,
                    CornerRadius = 10,
                    BorderColor = (_selectedIndex == currentIndex) ? Colors.Blue : Colors.Gray,
                    BackgroundColor = (_selectedIndex == currentIndex) ? Colors.LightBlue : Colors.Transparent,
                    Content = new VerticalStackLayout
                    {
                        Spacing = 5,
                        Children =
                        {
                            new Label
                            {
                                Text = practice.DisplayText,
                                FontSize = 18,
                                FontAttributes = FontAttributes.Bold
                            }
                        }
                    }
                };

                // Tap Gesture for Selection
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) => OnPresetSelected(currentIndex);
                practiceFrame.GestureRecognizers.Add(tapGesture);

                PlaylistStackLayout.Children.Add(practiceFrame);
            }

            PlayAllButton.IsEnabled = true;
        }

        private void OnPresetSelected(int index)
        {
            _selectedIndex = index; // Update selection

            // Refresh UI for selected item
            for (int i = 0; i < PlaylistStackLayout.Children.Count; i++)
            {
                if (PlaylistStackLayout.Children[i] is Frame frame)
                {
                    frame.BorderColor = (i == _selectedIndex) ? Colors.Blue : Colors.Gray;
                    frame.BackgroundColor = (i == _selectedIndex) ? Colors.LightBlue : Colors.Transparent;
                }
            }
        }

        private async void OnPlayAllClicked(object sender, EventArgs e)
        {
            if (_playlist.Count == 0) return;

            List<AudioSequence> allSequences = new List<AudioSequence>();

            foreach (var item in _playlist)
            {
                // ---------- NORMAL PRACTICE ----------
                if (item.Type == "Practice")
                {
                    var sequence = PracticeAudioMap.GetPracticeSequence(item.PracticeName);

                    if (sequence != null)
                    {
                        foreach (var step in sequence)
                        {
                            if (step.LoopDuration == -1)
                            {
                                allSequences.Add(new AudioSequence(
                                    step.FileName,
                                    item.DurationMinutes * 60,
                                    step.PlayerLoop,
                                    item.PracticeName
                                    ));
                            }
                            else
                            {
                                allSequences.Add(new AudioSequence(
                                    step.FileName,
                                    step.LoopDuration,
                                    step.PlayerLoop,
                                    item.PracticeName
                                ));
                            }
                        }
                    }
                }

                // ---------- CUSTOM PRACTICE ----------
                else if (item.Type == "CustomPractice" && item.TimerPreset != null)
                {
                    var timer = item.TimerPreset;

                    for (int i = 0; i < timer.TotalIntervals; i++)
                    {
                        // Interval
                        allSequences.Add(new AudioSequence(
                            "1Sec_Silence.mp3",
                            timer.IntervalDuration,
                            true,
                            item.PracticeName
                            ));

                        // Gap
                        if (i < timer.TotalIntervals - 1 && timer.GapBetweenIntervals > 0)
                        {
                            allSequences.Add(new AudioSequence(
                                "1Sec_Silence.mp3",
                                timer.GapBetweenIntervals,                                
                                true, item.PracticeName
                                ));
                        }
                    }

                    // End bell
                    allSequences.Add(new AudioSequence(
                        "3Bells.mp3",
                        0,
                        false));
                }
            }

            // 🔹 Pass modified sequence with user-defined duration to PracticePlayerPage
            await Navigation.PushAsync(new PracticePlayerPage("Yoga Playlist", allSequences));
        }


        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (_selectedIndex < 0 || _selectedIndex >= _playlist.Count)
            {
                await DisplayAlert("Error", "Please select a practice to delete.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Delete Practice", $"Are you sure you want to remove '{_playlist[_selectedIndex].PracticeName}'?", "Yes", "No");
            if (!confirm) return;

            _playlist.RemoveAt(_selectedIndex);
            _selectedIndex = -1; // Reset selection
            await YogaPlaylistService.SavePlaylistAsync(_playlist);
            LoadPlaylist();
        }

        private async void OnMoveUpClicked(object sender, EventArgs e)
        {
            if (_selectedIndex <= 0)
            {
                await DisplayAlert("Error", "Cannot move this practice up.", "OK");
                return;
            }

            var temp = _playlist[_selectedIndex];
            _playlist[_selectedIndex] = _playlist[_selectedIndex - 1];
            _playlist[_selectedIndex - 1] = temp;

            _selectedIndex--; // Move selection up
            await YogaPlaylistService.SavePlaylistAsync(_playlist);
            LoadPlaylist();
        }

        private async void OnMoveDownClicked(object sender, EventArgs e)
        {
            if (_selectedIndex < 0 || _selectedIndex >= _playlist.Count - 1)
            {
                await DisplayAlert("Error", "Cannot move this practice down.", "OK");
                return;
            }

            var temp = _playlist[_selectedIndex];
            _playlist[_selectedIndex] = _playlist[_selectedIndex + 1];
            _playlist[_selectedIndex + 1] = temp;

            _selectedIndex++; // Move selection down
            await YogaPlaylistService.SavePlaylistAsync(_playlist);
            LoadPlaylist();
        }
    }
}

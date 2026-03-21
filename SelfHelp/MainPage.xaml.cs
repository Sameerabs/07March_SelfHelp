using Plugin.Maui.Audio;

namespace SelfHelp
{

    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            // Restore last entered duration
            var lastDuration = Preferences.Get("last_duration_minutes", string.Empty);
            if (!string.IsNullOrEmpty(lastDuration))
                DurationEntry.Text = lastDuration;
        }

        private async void OnPracticeClicked(object sender, EventArgs e)
        {
            if (sender is not Button button) return;
            if (button.CommandParameter is not string practiceName) return;

            if (!int.TryParse(DurationEntry.Text, out int durationMinutes) || durationMinutes <= 0)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid duration in minutes.", "OK");
                return;
            }
            Preferences.Set("last_duration_minutes", DurationEntry.Text);

            await Navigation.PushAsync(new PracticePlayerPage(practiceName, durationMinutes));
        }

        private async void OnPracticeMenuClicked(object sender, EventArgs e)
        {
            if (sender is not Button button || button.CommandParameter is not string practiceName) return;

            string action = await DisplayActionSheet("Choose Action", "Cancel", null, "▶ Play Now", "➕ Add to Playlist");

            if (action == "▶ Play Now")
            {
                if (!int.TryParse(DurationEntry.Text, out int durationMinutes) || durationMinutes <= 0)
                {
                    await DisplayAlert("Invalid Input", "Please enter a valid duration in minutes.", "OK");
                    return;
                }

                Preferences.Set("last_duration_minutes", DurationEntry.Text);
                await Navigation.PushAsync(new PracticePlayerPage(practiceName, durationMinutes));
            }

            else if (action == "➕ Add to Playlist")
            {
                AddPracticeToPlaylist(practiceName);
            }
        }
        private async void OnViewPlaylistClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new YogaPlaylistPage());
        }
        private async void OnAddTimerClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewTimerPage()); // Navigate to Add Timer Page
        }
        private async void AddPracticeToPlaylist(string practiceName)
        {
            if (!int.TryParse(DurationEntry.Text, out int durationMinutes) || durationMinutes <= 0)
            {
                await DisplayAlert("Invalid Input", "Please enter a valid duration in minutes.", "OK");
                return;
            }

            // 🔹 Load existing playlist
            var playlist = await YogaPlaylistService.LoadPlaylistAsync();

            // 🔹 Add new practice with user-defined duration
            playlist.Add(new PlaylistItem
            {
                Type = "Practice",
                PracticeName = practiceName,
                DurationMinutes = durationMinutes
            });

            // 🔹 Save the updated playlist
            await YogaPlaylistService.SavePlaylistAsync(playlist);

            Preferences.Set("last_duration_minutes", DurationEntry.Text);

            await DisplayAlert("Added", $"{practiceName} ({durationMinutes} min) has been added to your playlist!", "OK");
        }
        private async void OnViewStatsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StatisticsPage());
        }


    }
}

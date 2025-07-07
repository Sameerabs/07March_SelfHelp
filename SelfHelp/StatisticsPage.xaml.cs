using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SelfHelp
{
    public partial class StatisticsPage : ContentPage
    {
        private List<PracticeSession> _sessionHistory = new List<PracticeSession>();
        public ObservableCollection<PracticeSession> FilteredSessions { get; set; } = new ObservableCollection<PracticeSession>();
        public StatisticsPage()
        {
            InitializeComponent();
            LoadPracticeData();
        }

        private async void LoadPracticeData()
        {
            try
            {
                // ✅ Load the embedded JSON file from the Raw folder
                using var stream = await FileSystem.OpenAppPackageFileAsync("practice_stats.json");
                using var reader = new StreamReader(stream);
                string json = await reader.ReadToEndAsync();

                _sessionHistory = JsonSerializer.Deserialize<List<PracticeSession>>(json) ?? new List<PracticeSession>();



                // Load all data initially
                FilteredSessions.Clear();
                foreach (var session in _sessionHistory)
                {
                    if (DateTime.TryParse(session.Date, out DateTime parsedDate))
                    {
                        session.Date = parsedDate.ToString("dd-MMM-yy");  // ✅ Format date properly
                    }
                    FilteredSessions.Add(session);
                }


                // 🔹 Display Data in ListView
                SessionListView.ItemsSource = null;
                SessionListView.ItemsSource = FilteredSessions;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load practice data: {ex.Message}", "OK");
            }
        }


        public class PracticeSession
        {
            public string Date { get; set; }
            public string Time { get; set; }
            public string PracticeName { get; set; }
            public int DurationMinutes { get; set; }
            public bool Completed { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace SelfHelp
{
    public partial class StatisticsPage : ContentPage
    {
        private List<PracticeSession> _sessionHistory = new();
        public ObservableCollection<PracticeSession> FilteredSessions { get; set; } = new();

        public StatisticsPage()
        {
            InitializeComponent();
            SessionListView.ItemsSource = FilteredSessions; // ✅ Set once
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadPracticeData();
        }

        private async Task LoadPracticeData()
        {
            try
            {
                string filePath = Path.Combine(FileSystem.AppDataDirectory, "practice_stats.json");

                if (!File.Exists(filePath))
                {
                    FilteredSessions.Clear();
                    return;
                }

                string json = await File.ReadAllTextAsync(filePath);

                _sessionHistory = JsonSerializer.Deserialize<List<PracticeSession>>(json)
                                  ?? new List<PracticeSession>();

                var sorted = _sessionHistory
                    .OrderByDescending(s => DateTime.TryParse(s.Date, out var d) ? d : DateTime.MinValue)
                    .ThenByDescending(s => s.Time)
                    .ToList();

                FilteredSessions.Clear();

                foreach (var session in sorted)
                {
                    FilteredSessions.Add(session);
                }

                // 🔥 Ready for charts next step
                GenerateInsights();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load practice data: {ex.Message}", "OK");
            }
        }

        // 🔥 Prepare data for charts (next step)
        private void GenerateInsights()
        {
            if (_sessionHistory == null || _sessionHistory.Count == 0)
                return;

            var daily = _sessionHistory
                .GroupBy(s => s.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalMinutes = g.Sum(x => x.DurationMinutes)
                })
                .OrderBy(x => x.Date)
                .ToList();

            var mostPracticed = _sessionHistory
                .GroupBy(s => s.PracticeName)
                .Select(g => new
                {
                    Practice = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .ToList();

            var timePerPractice = _sessionHistory
                .GroupBy(s => s.PracticeName)
                .Select(g => new
                {
                    Practice = g.Key,
                    TotalMinutes = g.Sum(x => x.DurationMinutes)
                })
                .OrderByDescending(x => x.TotalMinutes)
                .ToList();

            // (Next step → bind these to charts)
        }
    }
}
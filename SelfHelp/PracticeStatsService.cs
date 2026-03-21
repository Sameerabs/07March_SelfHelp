using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHelp
{
    public static class PracticeStatsService
    {
        private static readonly string FilePath =
            Path.Combine(FileSystem.AppDataDirectory, "practice_stats.json");

        public static async Task<List<PracticeSession>> LoadAsync()
        {
            if (!File.Exists(FilePath))
                return new List<PracticeSession>();

            string json = await File.ReadAllTextAsync(FilePath);

            return JsonSerializer.Deserialize<List<PracticeSession>>(json)
                   ?? new List<PracticeSession>();
        }

        public static async Task SaveAsync(List<PracticeSession> sessions)
        {
            string json = JsonSerializer.Serialize(sessions, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await File.WriteAllTextAsync(FilePath, json);
        }

        public static async Task AddSessionAsync(PracticeSession session)
        {
            var sessions = await LoadAsync();
            sessions.Add(session);
            await SaveAsync(sessions);
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHelp
{
    public class JsonStorageService
    {
        private static readonly string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "timers.json");

        // 🔹 Save a list of timers to JSON
        public async Task SaveTimersAsync(List<TimerPreset> timers)
        {
            try
            {
                string json = JsonSerializer.Serialize(timers, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving timers: {ex.Message}");
            }
        }

        // 🔹 Load timers from JSON
        public async Task<List<TimerPreset>> LoadTimersAsync()
        {
            try
            {
                if (!File.Exists(filePath)) return new List<TimerPreset>();

                string json = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<List<TimerPreset>>(json) ?? new List<TimerPreset>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading timers: {ex.Message}");
                return new List<TimerPreset>();
            }
        }

        // 🔹 Delete a specific timer
        public async Task DeleteTimerAsync(string timerName)
        {
            try
            {
                var timers = await LoadTimersAsync();
                timers.RemoveAll(t => t.Name == timerName);
                await SaveTimersAsync(timers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting timer: {ex.Message}");
            }
        }
    }
}

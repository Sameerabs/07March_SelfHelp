using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace SelfHelp
{
    public static class YogaPlaylistService
    {
        private static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "yoga_playlist.json");

        // 🔹 Save Playlist to JSON (Corrected to store `PlaylistItem`)
        public static async Task SavePlaylistAsync(List<PlaylistItem> playlist)
        {
            string json = JsonSerializer.Serialize(playlist, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(FilePath, json);
        }

        // 🔹 Load Playlist from JSON
        public static async Task<List<PlaylistItem>> LoadPlaylistAsync()
        {
            if (!File.Exists(FilePath)) return new List<PlaylistItem>(); // ✅ Return empty if no file

            string json = await File.ReadAllTextAsync(FilePath);

            try
            {
                return JsonSerializer.Deserialize<List<PlaylistItem>>(json) ?? new List<PlaylistItem>();
            }
            catch (JsonException)
            {
                return new List<PlaylistItem>(); // ✅ Return empty list if file is corrupt
            }
        }
    }
}

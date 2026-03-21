using Plugin.Maui.Audio;

namespace SelfHelp
{
    public class AudioPlayerService
    {
        private static AudioPlayerService _instance;
        public static AudioPlayerService Instance => _instance ??= new AudioPlayerService();

        private IAudioPlayer _player;
        private Stream _currentStream; // ✅ Keep reference

        private AudioPlayerService() { }

        // 🔹 Play audio
        public async Task PlayAsync(Stream audioStream, bool loop)
        {
            Stop(); // Ensure only one audio plays at a time

            _currentStream = audioStream; // ✅ Store stream
            _player = AudioManager.Current.CreatePlayer(audioStream);
            _player.Loop = loop;
            _player.Play();
        }

        // 🔹 Stop audio
        public void Stop()
        {
            if (_player != null)
            {
                _player.Stop();
                _player.Dispose();
                _player = null;
            }

            // ✅ Dispose stream properly
            _currentStream?.Dispose();
            _currentStream = null;
        }

        // 🔹 Pause audio
        public void Pause()
        {
            _player?.Pause();
        }

        // 🔹 Check if playing
        public bool IsPlaying => _player?.IsPlaying ?? false;
    }
}
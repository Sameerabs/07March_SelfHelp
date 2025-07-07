using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;

[Service(ForegroundServiceType = ForegroundService.TypeMediaPlayback)]
public class AudioService : Service
{
    private MediaPlayer _mediaPlayer;
    private const string CHANNEL_ID = "AudioServiceChannel";

    public override void OnCreate()
    {
        base.OnCreate();
        CreateNotificationChannel();
        _mediaPlayer = new MediaPlayer();
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        string audioPath = intent.GetStringExtra("AudioPath");

        if (!string.IsNullOrEmpty(audioPath))
        {
            try
            {
                _mediaPlayer.Reset();
                _mediaPlayer.SetDataSource(audioPath);
                _mediaPlayer.Prepare();
                _mediaPlayer.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        var notification = new NotificationCompat.Builder(this, CHANNEL_ID)
            .SetContentTitle("Yoga & Meditation Playing")
            .SetContentText("Your guided practice is running...")
            .SetSmallIcon(Android.Resource.Drawable.IcMediaPlay)
            .Build();

        StartForeground(1, notification);
        return StartCommandResult.Sticky;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        _mediaPlayer?.Stop();
        _mediaPlayer?.Release();
    }

    public override IBinder OnBind(Intent intent) => null;

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            var channel = new NotificationChannel(CHANNEL_ID, "Audio Service Channel", NotificationImportance.Low);
            var manager = GetSystemService(NotificationService) as NotificationManager;
            manager?.CreateNotificationChannel(channel);
        }
    }
}

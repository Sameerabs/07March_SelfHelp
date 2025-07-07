namespace SelfHelp
{
    public class TimerPreset
    {
        public string Name { get; set; } // Timer name

        public int IntervalDuration { get; set; } // In seconds

        public int TotalIntervals { get; set; } // 1-99

        public int GapBetweenIntervals { get; set; } // In seconds
    }
}


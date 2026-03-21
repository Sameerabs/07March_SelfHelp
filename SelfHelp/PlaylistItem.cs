using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHelp
{
    public class PlaylistItem
    {
        // Practice or CustomPractice
        public string Type { get; set; }

        // Used when Type = Practice
        public string PracticeName { get; set; }

        public int DurationMinutes { get; set; }

        // Used when Type = CustomPractice
        public TimerPreset TimerPreset { get; set; }

        public string DisplayText
        {
            get
            {
                // Normal practice
                if (Type == "Practice" || (string.IsNullOrEmpty(Type) && !string.IsNullOrEmpty(PracticeName)))
                    return $"{PracticeName} ({DurationMinutes} min)";

                // Custom practice (timer)
                if (Type == "CustomPractice" && TimerPreset != null)
                {
                    int totalSeconds =
                        (TimerPreset.IntervalDuration * TimerPreset.TotalIntervals) +
                        (TimerPreset.GapBetweenIntervals * (TimerPreset.TotalIntervals - 1));

                    int totalMinutes = totalSeconds / 60;

                    return $"{TimerPreset.Name} ({totalMinutes} min)";
                }

                return "Unknown Item";
            }
        }
    }
}


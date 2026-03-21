using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHelp
{
    public static class TimerSequenceBuilder
    {
        public static List<AudioSequence> ConvertPresetToAudioSequence(TimerPreset preset)
        {
            List<AudioSequence> sequence = new();

            for (int i = 0; i < preset.TotalIntervals; i++)
            {
                sequence.Add(new AudioSequence("IntervalBeep.mp3", preset.IntervalDuration, true));

                if (i < preset.TotalIntervals - 1)
                {
                    sequence.Add(new AudioSequence("Silence.mp3", preset.GapBetweenIntervals, true));
                }
            }

            sequence.Add(new AudioSequence("EndChime.mp3", 0, false));

            return sequence;
        }
    }
}

using System.Collections.Generic;

namespace SelfHelp
{
    public static class PracticeAudioMap
    {
        private static readonly Dictionary<string, List<AudioSequence>> _practiceSequences = new()
        {
            {
                "Isha Kriya", new List<AudioSequence>
                {
                   new AudioSequence("1Sec_Silence.mp3", loopDuration: 15, playerLoop: true), // Fixed 15 Seconds 
                   new AudioSequence("IamNotTheBody.mp3", loopDuration: -1, playerLoop: true), // Loop as per user input
                    new AudioSequence("Isha_Kriya_End.mp3", loopDuration: 0, playerLoop: false)
                }
            },
            {
                "Daily Sadhana1", new List<AudioSequence>
                {
                    new AudioSequence("Flute_Shanti.mp3", loopDuration: 0, playerLoop: false), // Loop as per user input
                    new AudioSequence("IamNotTheBody.mp3", loopDuration: 900, playerLoop: true), // Loop as per user input
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 300, playerLoop: true), // Fixed 10 min duration
                    new AudioSequence("Flute_Shanti.mp3", loopDuration: 0, playerLoop: false), // Loop as per user input
                    new AudioSequence("IamNotTheBody.mp3", loopDuration: 900, playerLoop: true), // Loop as per user input
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 300, playerLoop: true), // Fixed 10 min duration
                    new AudioSequence("Flute_Shanti.mp3", loopDuration: 0, playerLoop: false), // Loop as per user input
                    new AudioSequence("IamNotTheBody.mp3", loopDuration: 900, playerLoop: true), // Loop as per user input
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 300, playerLoop: true), // Fixed 10 min duration
                    new AudioSequence("Flute_Shanti.mp3", loopDuration: 0, playerLoop: false), // Loop as per user input
                    new AudioSequence("IamNotTheBody.mp3", loopDuration: 900, playerLoop: true), // Loop as per user input
                    new AudioSequence("Isha_Kriya_End.mp3", loopDuration: 0, playerLoop: false),
                }
            },

            {
                "Daily Sadhana",  GenerateDailySadhanaSequence()

            },

            {
                "Nadi Shidhi", new List<AudioSequence>
                {
                     new AudioSequence("1Sec_Silence.mp3", loopDuration: 15, playerLoop: true), // Fixed 15 Seconds 
                   new AudioSequence("Nadishudhi.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: -1, playerLoop: true), // Loop as per user input
                    new AudioSequence("LeftNastril_Stop.mp3", loopDuration: 0, playerLoop: false)
                }
            },


            {
                "Silence", new List<AudioSequence>
                {
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 15, playerLoop: true), // Fixed 2 min duration
                    new AudioSequence("Bell1.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: -1, playerLoop: true), // Loop as per user input
                    new AudioSequence("Bell1.mp3", loopDuration: 0, playerLoop: false),
                }
            },


            {
                "Sukha Kriya", new List<AudioSequence>
                {
                    new AudioSequence("SukhaKriya.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: -1, playerLoop: true), // Loop as per user input
                    new AudioSequence("LeftNastril_Stop.mp3", loopDuration: 0, playerLoop: false)
                }
            },
            {
                "AumNamahshivaya", new List<AudioSequence>
                {
                   new AudioSequence("1Sec_Silence.mp3", loopDuration: 15, playerLoop: true), // Fixed 15 Seconds 
                    new AudioSequence("Bell1.mp3", loopDuration: 0, playerLoop: false),
                   new AudioSequence("1Sec_Silence.mp3", loopDuration: 5, playerLoop: true), // Fixed 15 Seconds 
                    new AudioSequence("AumNamhShivaiah.mp3", loopDuration: -1, playerLoop: true), // Loop as per user input
                    new AudioSequence("Aum_Shanti.mp3", loopDuration: 0, playerLoop: false),
                }
            },

            {
                "Patangasana", new List<AudioSequence>
                {
                    new AudioSequence("Patangasana.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 120, playerLoop: true), // Fixed 2 min duration
                    new AudioSequence("Relax.mp3", loopDuration: 0, playerLoop: false)
                }
            },

            {
                "AumChanting", new List<AudioSequence>
                {
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 5, playerLoop: true), // Fixed 2 min duration
                    new AudioSequence("AumChanting_25ec.mp3", loopDuration: -1, playerLoop: true), // Loop as per user input
                    new AudioSequence("Aum_Shanti.mp3", loopDuration: 0, playerLoop: false),
                }
            },
            {
                "Shambhavi", new List<AudioSequence>
                {
                    new AudioSequence("IE_Crash_Course.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("SukhaKriya.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 600, playerLoop: true), // Fixed 10 min duration
                    new AudioSequence("YogaMudra.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 5, playerLoop: true),
                    new AudioSequence("AumChanting_25ec.mp3", loopDuration: 900, playerLoop: false),
                    new AudioSequence("Vipareeta_swasa.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("1Sec_Silence.mp3", loopDuration: 300, playerLoop: true), // Fixed 10 min duration
                   new AudioSequence("Bhandaas.mp3", loopDuration: 0, playerLoop: false),
                    new AudioSequence("Flute_Shanti.mp3", loopDuration: 0, playerLoop: false),
                }
            },

        };

        private static List<AudioSequence> GenerateDailySadhanaSequence()
        {
            var sequence = new List<AudioSequence>();
            sequence.Add(new AudioSequence("Flute_Shanti.mp3", loopDuration: 0, playerLoop: false));

            for (int i = 0; i < 15; i++)
                sequence.Add(new AudioSequence("IamNotTheBody.mp3", loopDuration: 60, playerLoop: true));

            for (int i = 0; i < 5; i++)
                sequence.Add(new AudioSequence("1Sec_Silence.mp3", loopDuration: 60, playerLoop: true));

            sequence.Add(new AudioSequence("Flute_Shanti.mp3", loopDuration: 0, playerLoop: false));

            for (int i = 0; i < 15; i++)
                sequence.Add(new AudioSequence("IamNotTheBody.mp3", loopDuration: 60, playerLoop: true));

            for (int i = 0; i < 5; i++)
                sequence.Add(new AudioSequence("1Sec_Silence.mp3", loopDuration: 60, playerLoop: true));

            sequence.Add(new AudioSequence("Flute_Shanti.mp3", loopDuration: 0, playerLoop: false));

            for (int i = 0; i < 15; i++)
                sequence.Add(new AudioSequence("IamNotTheBody.mp3", loopDuration: 60, playerLoop: true));

            for (int i = 0; i < 5; i++)
                sequence.Add(new AudioSequence("1Sec_Silence.mp3", loopDuration: 60, playerLoop: true));

            sequence.Add(new AudioSequence("Isha_Kriya_End.mp3", loopDuration: 0, playerLoop: false));

            return sequence;
        }

        public static List<AudioSequence> GetPracticeSequence(string practiceName)
        {
            return _practiceSequences.TryGetValue(practiceName, out var sequence) ? sequence : null;
        }
    }

    public class AudioSequence
    {
        public string FileName { get; }
        public int LoopDuration { get; } // -1 for user-defined duration
        public bool PlayerLoop { set; get; } // 

        public AudioSequence(string fileName, int loopDuration, bool playerLoop)
        {
            FileName = fileName;
            LoopDuration = loopDuration;
            PlayerLoop = playerLoop;
        }
    }

}

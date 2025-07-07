using System.Collections.Generic;

namespace SelfHelp
{
    public static class AudioMetadata
    {
        // 🔹 Dictionary storing the length of each audio file (avoiding duplication)
        public static readonly Dictionary<string, int> AudioLengths = new()
        {
            { "1Sec_Silence.mp3", 1 },  
            { "AumNamhShivaiah.mp3", 7 },      
            { "Flute_Shanti.mp3", 94 },     
            { "IamNotTheBody.mp3", 13 },       
            { "Isha_Kriya_End.mp3", 292 },       
            { "Patangasana.mp3", 5 },
            { "AumChanting.mp3", 15 },
             { "AumChanting_25ec.mp3", 25 },
           { "Aum_Shanti.mp3", 15 },
           { "IE_Crash_Course.mp3", 44 },
           { "SukhaKriya.mp3", 15 },
           { "YogaMudra.mp3", 1 },
           { "Vipareeta_swasa.mp3", 1 },
           { "Bhandaas.mp3", 1 },
           { "Nadishudhi.mp3", 3 },
           { "LeftNastril_Stop.mp3", 3 },
           { "Bell1.mp3", 5 },
           { "Bell_Transition.mp3", 10 },
           { "2Bells.mp3", 2 },
           { "3Bells.mp3" , 2 },
           { "small_Bell_Sound.mp3" , 1 },
      };

        // 🔹 Get audio length safely (returns 0 if not found)
        public static int GetAudioLength(string fileName)
        {
            return AudioLengths.TryGetValue(fileName, out int length) ? length : 0;
        }
    }
}

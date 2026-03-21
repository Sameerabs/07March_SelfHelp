using System.Collections.Generic;

namespace SelfHelp
{
    public static class AudioMetadata
    {
        // 🔹 Dictionary storing the length of each audio file (avoiding duplication)
        public static readonly Dictionary<string, double> AudioLengths = new()
{
    { "1Sec_Silence.mp3", 1.0 },
    { "AumNamhShivaiah.mp3", 6.99 },
    { "Flute_Shanti.mp3", 94.26 },
    { "IamNotTheBody.mp3", 12.90 },
    { "Isha_Kriya_End.mp3", 291.83 },
    { "Patangasana.mp3", 5.00 },
    { "AumChanting.mp3", 15.01 },
    { "AumChanting_25ec.mp3", 25.01 },
    { "Aum_Shanti.mp3", 15.03 },
    { "IE_Crash_Course.mp3", 44.27 },
    { "SukhaKriya.mp3", 15.00 },
    { "YogaMudra.mp3", 1.00 },
    { "Vipareeta_swasa.mp3", 2.00 },
    { "Bhandaas.mp3", 1.01 },
    { "Nadishudhi.mp3", 2.98 },
    { "LeftNastril_Stop.mp3", 3.01 },
    { "Bell1.mp3", 2.00 },   // ⚠️ FIXED (was 5 ❌)
    { "Bell_Transition.mp3", 10.06 },
    { "2Bells.mp3", 1.50 },
    { "3Bells.mp3", 2.00 },
    { "small_Bell_Sound.mp3", 0.50 },
    { "Open_Your_Eyes.mp3", 14.67 },
    { "Peace_Flute.mp3", 93.45 },
    { "Punarapi_Maranam.mp3", 187.41 },
};

        // 🔹 Get audio length safely (returns 0 if not found)
        public static double GetAudioLength(string fileName)
        {
            return AudioLengths.TryGetValue(fileName, out double length) ? length : 0;
        }
    }
}

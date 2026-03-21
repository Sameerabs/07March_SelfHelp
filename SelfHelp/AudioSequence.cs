using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHelp
{
    public class AudioSequence
    {
        public string FileName { get; }
        public int LoopDuration { get; }
        public bool PlayerLoop { get; set; }

        public string SourceName { get; set; }   // NEW
        public string PracticeName { get; set; }

        public AudioSequence(string fileName, int loopDuration, bool playerLoop, string sourceName = "", string practiceName = "")
        {
            FileName = fileName;
            LoopDuration = loopDuration;
            PlayerLoop = playerLoop;
            SourceName = sourceName;
            PracticeName = practiceName;
        }
    }
}

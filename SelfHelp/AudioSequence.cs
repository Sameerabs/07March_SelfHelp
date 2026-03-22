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

        public string PracticeName { get; set; }
        public string SourceName { get; set; }   // NEW

        public AudioSequence(string fileName, int loopDuration, bool playerLoop, string practiceName = "", string sourceName = "" )
        {
            FileName = fileName;
            LoopDuration = loopDuration;
            PlayerLoop = playerLoop;
            PracticeName = practiceName;
            SourceName = sourceName;

        }
    }
}

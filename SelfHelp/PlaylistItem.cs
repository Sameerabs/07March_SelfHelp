using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfHelp
{
    public class PlaylistItem
    {
        public string PracticeName { get; set; }
        public int DurationMinutes { get; set; }

        // 🔹 DisplayText property to format Name + Duration
        public string DisplayText => $"{PracticeName} ({DurationMinutes} min)";
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace FindMySteamDLC.Models
{
    public class Game : App
    { 
        public List<Dlc> Dlcs { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FindMySteamDLC.Models
{
    public class Dlc : Media
    {
        public Dlc(Game game) { this.Game = game; }

        public Game Game { get; set; }
    }
}

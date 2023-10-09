using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.ComponentModel.DataAnnotations;
using FindMySteamDLC.Handlers;

namespace FindMySteamDLC.Models
{
    public abstract class App
    {
        public int AppID { get; set; }
        public string Name { get; set; } 
        public bool IsInstalled { get; set; }
    }
}

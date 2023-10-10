using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using System.ComponentModel.DataAnnotations;

namespace FindMySteamDLC.Models
{
    public abstract class App
    {
        [Key]
        public int AppID { get; set; }
        public string Name { get; set; } 
        public bool IsInstalled { get; set; }
    }
}

using FindMySteamDLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMySteamDLC.src.Services
{
    public interface ISteamService
    {
        public IEnumerable<Game> GetGamesFromFiles(string pathToSteam);
        public bool GetDlcsFromFiles(int AppID);
        public string GetSteamRepository();
    }
}

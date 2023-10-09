using FindMySteamDLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMySteamDLC.Services
{
    public interface ISteamService
    {
        public Task<IEnumerable<Game>> GetGamesFromFiles(string pathToSteam);
        public string GetSteamRepository();
    }
}

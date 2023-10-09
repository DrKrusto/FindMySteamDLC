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
        public IEnumerable<Game> GetGamesFromFiles(string pathToSteam);
        public Task<IEnumerable<Dlc>> GetDlcsFromSteamWeb(int appID, Game assignedTo = null, int[] appidToSkip = null);
        public string GetSteamRepository();
    }
}

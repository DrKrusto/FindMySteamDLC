using FindMySteamDLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMySteamDLC.Services
{
    public interface ISteamRepository
    {
        public void AddGame(Game game);
        public void AddDlc(Dlc dlc);
        public void AddDlcs(IEnumerable<Dlc> dlcs);
        public void AddGames(IEnumerable<Game> games);
        public Game GetGame(int appID);
        public Dlc GetDlc(int appID);
        public IEnumerable<Game> GetGames();
        public IEnumerable<Dlc> GetDlcsForGame(int appID);
        public void UpdateAllDlcsMetadata();
        public void AddDlcsFromSteamWeb();
        public void AddDlcsFromSteamWeb(int appID);
        public void AddAppsFromFiles(string pathToSteam);
    }
}

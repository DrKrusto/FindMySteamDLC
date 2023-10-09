using FindMySteamDLC.Data;
using FindMySteamDLC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMySteamDLC.Services
{
    public class SteamRepository : ISteamRepository
    {
        private readonly SteamDbContext context;
        private readonly ISteamService steamService;

        public SteamRepository(SteamDbContext context, ISteamService steamService)
        {
            this.context = context;
            this.steamService = steamService;
        }

        public async void AddAppsFromFiles(string pathToSteam)
        {
            var games = await steamService.GetGamesFromFiles(pathToSteam);
            var dlcs = games.SelectMany(game => game.Dlcs);

            this.AddGames(games);
            this.AddDlcs(dlcs);
        }

        public void AddDlc(Dlc dlc)
        {
            this.context.Add(dlc);
            this.context.SaveChanges();
        }

        public void AddDlcs(IEnumerable<Dlc> dlcs)
        {
            this.context.AddRange(dlcs);
            this.context.SaveChanges();
        }

        public void AddGame(Game game)
        {
            this.context.Add(game);
            this.context.SaveChanges();
        }

        public void AddGames(IEnumerable<Game> games)
        {
            this.context.AddRange(games);
            this.context.SaveChanges();
        }

        public Dlc GetDlc(int appID)
        {
            return this.context
                .Dlcs
                .FirstOrDefault(dlc => dlc.AppID == appID);
        }

        public IEnumerable<Dlc> GetDlcsForGame(int appID)
        {
            return this.context
                .Dlcs
                .Where(dlc => dlc.Game.AppID == appID)
                .ToList();
        }

        public Game GetGame(int appID)
        {
            return this.context
                .Games
                .FirstOrDefault(game => game.AppID == appID);
        }

        public IEnumerable<Game> GetGames()
        {
            return this.context.Games.ToList();
        }
    }
}

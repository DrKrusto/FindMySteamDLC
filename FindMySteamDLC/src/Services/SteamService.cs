using FindMySteamDLC.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FindMySteamDLC.Services
{
    public class SteamService : ISteamService
    {
        private readonly ILogger<SteamService> logger;

        private readonly List<string> SteamRegistryPaths = new()
        {
            @"SOFTWARE\Valve\Steam",
            @"SOFTWARE\Wow6432Node\Valve\Steam"
        };

        public SteamService(ILogger<SteamService> logger)
        {
            this.logger = logger;
        }

        public string GetSteamRepository()
        {
            foreach (string path in SteamRegistryPaths)
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(path);
                if (key != null)
                    return key.GetValue("InstallPath").ToString();
            }
            return null;
        }

        public async Task<IEnumerable<Game>> GetGamesFromFiles(string pathToSteam)
        {
            var foundGames = new List<Game>();
            foreach (string filePath in Directory.EnumerateFiles(Path.Combine(pathToSteam, "steamapps")))
            {
                if (!filePath.EndsWith(".acf"))
                {
                    continue;
                }

                Game game = await ParseGameFromFile(filePath);
                foundGames.Add(game);
            }
            return foundGames;
        }

        private async Task<Game> ParseGameFromFile(string filePath)
        {
            Game game = null;

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    game = new Game { IsInstalled = true };

                    while (!reader.EndOfStream)
                    {
                        string line = await reader.ReadLineAsync();
                        
                        var regex = new Regex(@"(appid|name|dlcappid)");
                        Match match = regex.Match(line);
                        if (match.Success)
                        {
                            switch (match.Groups[0].Value)
                            {
                                case "appid":
                                    game.AppID = ExtractAppID(line);
                                    break;
                                case "name":
                                    game.Name = ExtractName(line);
                                    break;
                                case "dlcappid":
                                    int dlcAppID = ExtractDlcAppID(line);
                                    Dlc dlc = new() { AppID = dlcAppID, IsInstalled = true, Game = game };
                                    game.Dlcs.Add(dlc);
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Error while parsing a game file for the file \"{filePath}\"", ex.Message);
            }

            return game;
        }

        private int ExtractAppID(string line)
        {
            int appID = 0;
            try
            {
                appID = Convert.ToInt32(line.Split('"')[3].Trim());
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error while extracting a game AppID", ex.Message);
            }
            return appID;
        }

        private string ExtractName(string line)
        {
            string name = String.Empty;
            try
            {
                name = line.Split('"')[3].Trim();
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error while extracting a game name", ex.Message);
            }
            return name;
        }

        private int ExtractDlcAppID(string line)
        {
            int appID = 0;
            try
            {
                appID = Convert.ToInt32(line.Split('"')[3].Trim());
            }
            catch (Exception ex)
            {
                this.logger.LogError("Error while extracting a DLC AppID", ex.Message);
            }
            return appID;
        }
    }
}

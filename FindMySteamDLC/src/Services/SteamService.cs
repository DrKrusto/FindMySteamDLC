using FindMySteamDLC.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindMySteamDLC.src.Services
{
    public class SteamService : ISteamService
    {
        public SteamService()
        {

        }

        public bool GetDlcsFromFiles(int AppID)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Game> GetGamesFromFiles(string pathToSteam)
        {
            throw new NotImplementedException();
        }

        public string GetSteamRepository()
        {
            // TODO: not hardcode the paths
            List<string> paths = new List<string>
            {
                @"SOFTWARE\Valve\Steam",
                @"SOFTWARE\Wow6432Node\Valve\Steam"
            };

            foreach (string path in paths)
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(path);
                if (key != null)
                    return key.GetValue("InstallPath").ToString();
            }

            return null;
        }
    }
}

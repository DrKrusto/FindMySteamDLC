using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMySteamDLC.Services
{
    public interface ISteamWebService
    {
        public Task<IEnumerable<Models.Dlc>> GetDlcsFromSteamWeb(Models.Game assignedTo, int[] appidToSkip = null);
    }
}

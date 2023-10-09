using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindMySteamDLC.Services
{
    public interface ISteamWebService
    {
        public Task<IEnumerable<Models.Dlc>> GetDlcsFromSteamWeb(int appID, Models.Game assignedTo = null, int[] appidToSkip = null);
    }
}

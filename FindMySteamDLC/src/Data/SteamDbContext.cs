using FindMySteamDLC.Models;
using Microsoft.EntityFrameworkCore;

namespace FindMySteamDLC.Data
{
    public class SteamDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Dlc> Dlcs { get; set; }
    }
}

using FindMySteamDLC.Models;
using Microsoft.EntityFrameworkCore;

namespace FindMySteamDLC.Data
{
    public class SteamDbContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Dlc> Dlcs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=GamesData.db");
        }
    }
}

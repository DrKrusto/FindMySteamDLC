using FindMySteamDLC.Data;
using FindMySteamDLC.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Application = System.Windows.Application;

namespace FindMySteamDLC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider serviceProvider;

        public App()
        {
            #region Database
            using (var context = new SteamDbContext())
            {
                context.Database.Migrate();
            }
            #endregion

            #region Dependency Injection
            ServiceCollection services = new ServiceCollection();

            // Register services
            services.AddTransient<WebClient>();
            services.AddTransient<ISteamService, SteamService>();
            services.AddDbContext<SteamDbContext>(options =>
            {
                options.UseSqlite("Data Source=GamesData.db");
            }); 
            

            // Add the services to be injected into the classes
            //services.AddTransient<SteamGameInfo>();

            serviceProvider = services.BuildServiceProvider();
            #endregion
        }
    }
}

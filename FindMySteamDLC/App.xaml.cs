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
            #region Dependency Injection
            ServiceCollection services = new ServiceCollection();

            services.AddTransient<ISteamService, SteamService>();
            services.AddTransient<ISteamWebService, SteamWebService>();
            services.AddTransient<ISteamRepository, SteamRepository>();
            services.AddDbContext<SteamDbContext>(); 

            serviceProvider = services.BuildServiceProvider();
            #endregion

            #region Database
            using (var context = new SteamDbContext())
            {
                context.Database.Migrate();
            }
            #endregion
        }
    }
}

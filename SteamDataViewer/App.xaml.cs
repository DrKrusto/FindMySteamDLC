using FindMySteamDLC.Data;
using FindMySteamDLC.Services;
using FindMySteamDLC.View;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace SteamDataViewer
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

            // Services
            services.AddTransient<ISteamService, SteamService>();
            services.AddTransient<ISteamWebService, SteamWebService>();
            services.AddTransient<ISteamRepository, SteamRepository>();

            // Data & Logging
            services.AddDbContext<SteamDbContext>();
            services.AddLogging();

            // Windows
            services.AddSingleton<Library>();

            serviceProvider = services.BuildServiceProvider();
            #endregion

            #region Database
            using (var context = new SteamDbContext())
            {
                context.Database.Migrate();
            }
            #endregion
        }

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            var testing = serviceProvider.GetRequiredService<Library>();
            testing.Show();

            base.OnStartup(e);
        }
    }
}

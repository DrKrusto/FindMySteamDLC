using FindMySteamDLC.Services;
using FindMySteamDLC.src.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace FindMySteamDLC
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();

            // Register services
            services.AddTransient<WebClient>();
            services.AddTransient<ISteamService, SteamService>();
            

            // Add the services to be injected into the classes
            //services.AddTransient<SteamGameInfo>();

            _serviceProvider = services.BuildServiceProvider();
        }
    }
}

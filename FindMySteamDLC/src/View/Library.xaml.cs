using FindMySteamDLC.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FindMySteamDLC.View
{
    /// <summary>
    /// Logique d'interaction pour Library.xaml
    /// </summary>
    public partial class Library : Window
    {
        private readonly ISteamService steamService;
        private readonly ISteamRepository steamRepository;

        public string SteamPath { get; set; }

        public Library(ISteamService steamService, ISteamRepository steamRepository)
        {
            this.steamService = steamService;
            this.steamRepository = steamRepository;
            this.SteamPath = steamService.GetSteamRepository();

            InitializeComponent();

            this.Main.Content = new HomePage(steamService, steamRepository);
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && this.IsActive)
            {
                DragMove();
            }
        }

        private void ExitApplication(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void MinimizeApplication(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}

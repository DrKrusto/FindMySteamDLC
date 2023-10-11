using FindMySteamDLC.Models;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FindMySteamDLC.Services;

namespace FindMySteamDLC.View
{
    /// <summary>
    /// Logique d'interaction pour HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private readonly ISteamService steamService;
        private readonly ISteamRepository steamRepository;

        private readonly string defaultSteamPath;
        private List<Game> games;

        public HomePage()
        {
            InitializeComponent();
        }

        public HomePage(ISteamService steamService, ISteamRepository steamRepository)
        {
            this.steamService = steamService;
            this.steamRepository = steamRepository;

            this.defaultSteamPath = steamService.GetSteamRepository();
            this.games = new();

            InitializeComponent();
            this.lb_games.ItemsSource = games;
        }

        private void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //this.label_dlcnotfound.Opacity = 0;
            //if (this.lb_games.SelectedIndex != -1)
            //{
            //    this.img_gameImage.Opacity = 100;
            //    Game game = (Game)this.lb_games.SelectedItem;
            //    this.img_gameImage.Source = new BitmapImage(new Uri(game.PathToImage));
            //    List<Dlc> dlcs = new List<Dlc>();
            //    foreach (var dlc in game.Dlcs)
            //    {
            //        if (dlc.Value.Name != "null")
            //        {
            //            dlcs.Add(dlc.Value);
            //        }
            //    }
            //    this.lb_dlcs.ItemsSource = dlcs;
            //    if (dlcs.Count == 0)
            //    {
            //        this.label_dlcnotfound.Opacity = 100;
            //    }
            //}
            //else
            //{
            //    this.img_gameImage.Opacity = 0;
            //}
        }

        private async void SearchGames(object sender, MouseButtonEventArgs e)
        {
            VistaFolderBrowserDialog openFileDialog = new VistaFolderBrowserDialog() { Description = "Select the Steam root folder", SelectedPath = defaultSteamPath, UseDescriptionForTitle = true };
            if (openFileDialog.ShowDialog() == true)
            {
                await Task.Run(() => { 
                    steamRepository.AddAppsFromFiles(openFileDialog.SelectedPath); 
                    this.games = steamRepository.GetGames().ToList();
                });
            }
        }

        private void ShowSteamPageForDlc(object sender, MouseButtonEventArgs e)
        {
            //Dlc selectedDlc = (Dlc)this.lb_dlcs.SelectedItem;
            //System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = String.Format("https://store.steampowered.com/app/{0}", selectedDlc.AppID), UseShellExecute = true });
        }

        private void SeekGamesFromDirectories(List<string> directories)
        {
            //foreach (string s in directories)
            //{
            //    AddGamesFromDirectory(s);
            //}
        }

        async private void AddGamesFromDirectory(string pathToDirectory)
        {
            //if (Directory.Exists(String.Format(@"{0}\steamapps", pathToDirectory)))
            //{
            //    this.grid_loading.IsEnabled = true;
            //    await Task.Run(() => SteamInfo.FetchGamesFromSteamFiles(pathToDirectory));
            //    this.grid_loading.IsEnabled = false;
            //}
        }

        async private void SearchDlcs(object sender, MouseButtonEventArgs e)
        {
            //this.grid_loading.IsEnabled = true;
            //await Task.Run(() => SteamInfo.FetchAllNonInstalledDlc());
            //this.grid_loading.IsEnabled = false;
        }

        private void AddGame(object sender, MouseButtonEventArgs e)
        {
            //SearchGamesWindow searchGamesWindow = new SearchGamesWindow();
            //searchGamesWindow.Show();
        }
    }
}

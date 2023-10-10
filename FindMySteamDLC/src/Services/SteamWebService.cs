using FindMySteamDLC.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FindMySteamDLC.Services
{
    public class SteamWebService : ISteamWebService
    {
        private readonly ILogger<SteamWebService> logger;
        private readonly ISteamService steamService;

        private const string steamUrl = "https://store.steampowered.com/";
        private const string steamCdn  = "http://cdn.akamai.steamstatic.com/";

        public SteamWebService(ILogger<SteamWebService> logger, ISteamService steamService)
        {
            this.logger = logger;
            this.steamService = steamService;
        } 

        public async Task<IEnumerable<Dlc>> GetDlcsFromSteamWeb(Game assignedTo, int[] appidToSkip = null)
        {
            List<Dlc> dlcs = new();
            HtmlDocument htmlDoc = await GetDlcsHtmlPage(assignedTo.AppID);
            HtmlNodeCollection dlcNodes = htmlDoc.DocumentNode
                .SelectNodes("//div[contains(concat(' ', normalize-space(@class), ' '), ' recommendation ')]");

            if (dlcNodes == null)
            {
                this.logger.LogInformation($"No DLCs found for the game with the AppID: {assignedTo.AppID}");
                return dlcs;
            }

            foreach (HtmlNode node in dlcNodes)
            {
                int appid = Convert.ToInt32(node
                    .SelectSingleNode("div/a")
                    .Attributes["data-ds-appid"].Value);

                if (appidToSkip != null && Array.Exists(appidToSkip, element => element == appid))
                {
                    continue;
                }

                string dlcName = node
                    .SelectSingleNode("a/div/div[1]/div/span[1]")
                    .InnerText;

                Dlc dlc = assignedTo.Dlcs.FirstOrDefault(dlc => dlc.AppID == appid);
                if (dlc != null)
                {
                    dlc.Name = dlcName;
                }
                else
                {
                    dlc = new Dlc
                    {
                        AppID = appid,
                        Name = dlcName,
                        Game = assignedTo
                    };
                    dlcs.Add(dlc);
                }

                // TODO: Move this to a specific method to handle the image download
                var steamPath = steamService.GetSteamRepository();
                if (!File.Exists(String.Format(@"{0}\appcache\librarycache\{1}_header.jpg", steamPath, appid)))
                {
                    using (HttpClient client = new())
                    {
                        try
                        {
                            var imageStream = await client.GetStreamAsync($"{steamCdn}steam/apps/{appid}/header.jpg");
                            using (var fileStream = new FileStream(@$"{steamPath}\appcache\librarycache\{appid}_header.jpg", FileMode.Create, FileAccess.Write))
                            {
                                imageStream.CopyTo(fileStream);
                            }   
                        }
                        catch (Exception e)
                        {
                            this.logger.LogError($"Error when getting the DLC image for the app \"{appid}\"", e);
                        }
                    }

                }
            }

            return dlcs;
        }

        private async Task<HtmlDocument> GetDlcsHtmlPage(int appID)
        {
            string gameName = this.GetUriGameName(appID);
            string url = $"{steamUrl}/dlc/{appID}/{gameName}/ajaxgetfilteredrecommendations";

            string rawHtml = String.Empty;
            using (HttpClient client = new())
            {
                string json = await client.GetStringAsync(url);
                using (JsonDocument document = JsonDocument.Parse(json))
                {
                    rawHtml = document.RootElement.GetProperty("results_html").GetString();
                }
            }

            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(rawHtml);
            return htmlDoc;
        }

        private string GetUriGameName(int appID)
        {
            HtmlWeb htmlWeb = new();
            try
            {
                return htmlWeb.Load($"{steamUrl}/dlc/{appID}")
                    .DocumentNode
                    .SelectSingleNode("//div[contains(concat(' ', normalize-space(@class), ' '), ' curator_avatar_image ')]/a")
                    .Attributes["href"].Value
                    .Split('/')[5];
            }
            catch (Exception e)
            {
                this.logger.LogError("Error while trying to get the URI game name from Steam website", e.Message);
                return null;
            }
        }
    }
}

﻿using FindMySteamDLC.Handlers;
using FindMySteamDLC.Models;
using FindMySteamDLC.src.Services;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FindMySteamDLC.Services
{
    public class SteamWebService : ISteamWebService
    {
        private readonly ILogger<SteamWebService> logger;
        private const string steamUrl = "https://store.steampowered.com/";

        public async Task<IEnumerable<Dlc>> GetDlcsFromSteamWeb(int appID, Game assignedTo = null, int[] appidToSkip = null)
        {
            List<Dlc> dlcs = new();
            HtmlDocument htmlDoc = await GetDlcsHtmlPage(appID);
            HtmlNodeCollection dlcNodes = htmlDoc.DocumentNode
                .SelectNodes("//div[contains(concat(' ', normalize-space(@class), ' '), ' recommendation ')]");

            if (dlcNodes == null)
            {
                this.logger.LogInformation($"No DLCs found for the game with the AppID: {appID}");
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

                Dlc dlc = null;
                if (assignedTo != null)
                {
                    dlc = assignedTo.Dlcs.FirstOrDefault(dlc => dlc.AppID == appid);
                    if (dlc != null)
                    {
                        dlc.Name = dlcName;
                    }
                }
                else
                {
                    // TODO: Add the game to the database if it doesn't exist
                    dlc = new Dlc(assignedTo)
                    {
                        Name = dlcName,
                        AppID = appid,
                        IsInstalled = false
                    };
                }

                //game.Dlcs.Add(appid, dlc);

                if (!File.Exists(String.Format(@"{0}\appcache\librarycache\{1}_header.jpg", SteamInfo.PathToSteam, appid)))
                {
                    using (WebClient client = new WebClient())
                    {
                        try
                        {
                            client.DownloadFile(String.Format("http://cdn.akamai.steamstatic.com/steam/apps/{0}/header.jpg", appid), String.Format(@"{0}\appcache\librarycache\{1}_header.jpg", SteamInfo.PathToSteam, appid));
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Couldn't download the dlc image. Exception: " + e.Message);
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

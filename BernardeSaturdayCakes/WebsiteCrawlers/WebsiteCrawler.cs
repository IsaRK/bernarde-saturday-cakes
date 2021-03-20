using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;

namespace BernardeSaturdayCakes
{
    public class WebsiteCrawler
    {
        private const string HomePageUrlSetting = "homePageUrl";
        private HtmlWeb _webClient;
        private HtmlDocument _homePage;

        private bool _isSaturdayCakeUpToDate = false;
        private bool _isSaturdayPatisserieUpToDate = false;

        public bool IsWebsiteUpToDate => _isSaturdayCakeUpToDate && _isSaturdayPatisserieUpToDate;

        public WebsiteCrawler(HtmlWeb webClient)
        {
            _webClient = webClient;
            _homePage = webClient.Load(ConfigurationManager.AppSettings[HomePageUrlSetting]);
            CheckUpToDate();
        }

        internal IEnumerable<SubcontentCrawler> GetSubContentCrawlers()
        {
            foreach (HtmlNode node in _homePage.DocumentNode.SelectNodes("//button[@class='readmore']"))
            {
                string subContentUrl = node.GetAttributeValue("data-href", null);
                if (subContentUrl.ToLower().Contains("patisserie"))
                {
                    yield return new PatisserieSubcontentCrawler(_webClient, subContentUrl);
                }
                else if (subContentUrl.ToLower().Contains("cakes"))
                {
                    yield return new CakeSubcontentCrawler(_webClient, subContentUrl);
                }
                else
                {
                    throw new HtmlChangedException("No SubContent Found");
                }
            }
        }

        private void CheckUpToDate()
        {
            _isSaturdayCakeUpToDate = IsCakeUpToDate();
            _isSaturdayPatisserieUpToDate = IsPatisserieUpToDate();
        }

        private bool IsCakeUpToDate()
        {
            var cakeNode = _homePage.DocumentNode.SelectSingleNode("//*[text() = 'Cake de la semaine']");

            if (cakeNode == null) throw new HtmlChangedException(nameof(cakeNode));

            HtmlNode dateCake = cakeNode.NextSibling.NextSibling;
            if (dateCake == null) throw new HtmlChangedException(nameof(dateCake));

            var lastDateCake = DateTime.ParseExact(dateCake.InnerText[^5..], "dd/MM", CultureInfo.InvariantCulture);
            if (lastDateCake == null) throw new HtmlChangedException(nameof(lastDateCake));

            return lastDateCake > DateTime.Now;
        }

        private bool IsPatisserieUpToDate()
        {
            var patisserieNode = _homePage.DocumentNode.SelectSingleNode("//*[text() = 'Pâtisserie du Samedi']");

            if (patisserieNode == null) throw new HtmlChangedException(nameof(patisserieNode));

            HtmlNode datePatisserie = patisserieNode.NextSibling.NextSibling;
            if (datePatisserie == null) throw new HtmlChangedException(nameof(datePatisserie));

            var lastDatePatisserie = datePatisserie.InnerText.Replace("Le ", null);

            var cultureInfo = new CultureInfo("fr-FR");
            var lastDateTimePatisserie = DateTime.Parse(lastDatePatisserie, cultureInfo);

            return lastDateTimePatisserie > DateTime.Now;
        }
    }
}

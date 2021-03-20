using HtmlAgilityPack;
namespace BernardeSaturdayCakes
{
    public abstract class SubcontentCrawler
    {    
        private HtmlWeb _webClient;

        protected string url;
        protected string nodeImageSource;
        protected HtmlNode nodeTitle;
        protected HtmlNode nodeDescription;

        public SubcontentCrawler(HtmlWeb webClient, string subContentUrl)
        {
            _webClient = webClient;
            url = subContentUrl;
        }

        public abstract void PopulateBody(ref string body);

        public virtual void GetContent()
        {
            var saturdayPage = _webClient.Load(url);

            nodeTitle = saturdayPage.DocumentNode.SelectSingleNode("//div[@class='actualite__content__content mce-content-body']/h2");
            if (nodeTitle == null) throw new HtmlChangedException($"SaturdayPage {url} {nameof(nodeTitle)}");

            nodeDescription = saturdayPage.DocumentNode.SelectSingleNode("//div[@class='actualite__content__content mce-content-body']/p[not(img)]");
            if (nodeDescription == null) throw new HtmlChangedException($"SaturdayPage {url} {nameof(nodeDescription)}");

            nodeImageSource = saturdayPage.DocumentNode.SelectSingleNode("//header[@class='actualite__entete']/div[@class='image']/img").GetAttributeValue("src", null);
            if (nodeImageSource == null) throw new HtmlChangedException($"SaturdayPage {url} {nameof(nodeImageSource)}");
        }
    }
}

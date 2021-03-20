using HtmlAgilityPack;

namespace BernardeSaturdayCakes
{
    public class CakeSubcontentCrawler : SubcontentCrawler
    {
        public CakeSubcontentCrawler(HtmlWeb webClient, string url) :base(webClient, url)
        {
        }

        public override void PopulateBody(ref string body)
        {
            body = body.Replace("{cakeImageSource}", nodeImageSource)
                .Replace("{cakeTitle}", nodeTitle.OuterHtml)
                .Replace("{cakeDescription}", nodeDescription.OuterHtml)
                .Replace("{cakeLink}", url);
        }
    }
}

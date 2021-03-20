using HtmlAgilityPack;

namespace BernardeSaturdayCakes
{
    public class PatisserieSubcontentCrawler : SubcontentCrawler
    {
        public PatisserieSubcontentCrawler(HtmlWeb webClient, string url) :base(webClient, url)
        {
        }

        public override void PopulateBody(ref string body)
        {
            body = body.Replace("{patisserieImageSource}", nodeImageSource)
                .Replace("{patisserieTitle}", nodeTitle.OuterHtml)
                .Replace("{patisserieDescription}", nodeDescription.OuterHtml)
                .Replace("{patisserieLink}", url);
        }
    }
}

using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace BernardeSaturdayCakes
{
    public class Program
    {
		public static async Task<int> Main()
        {
			try
            {
				var services = ServiceProviderBuilder.GetServiceProvider();
				var options = services.GetRequiredService<IOptions<MySecretOptions>>();

				var webClient = new HtmlWeb();
				var websitecrawler = new WebsiteCrawler(webClient);

				if (websitecrawler.IsWebsiteUpToDate)
				{
					var messageManager = new MessageManager(options);
					var body = messageManager.GetTemplateBody();

					foreach (var subContentCrawler in websitecrawler.GetSubContentCrawlers())
                    {
						subContentCrawler.GetContent();
						subContentCrawler.PopulateBody(ref body);
					}
					
                    var message = messageManager.CreateMessage(body);
					await messageManager.SendEmail(message);

					return (int)ExitCode.Success;
				}
				else
                {
					return (int)ExitCode.WebSiteNotUpdated;
				}
			}
			catch (HtmlChangedException ex)
            {
				Console.WriteLine($"Error occured : {ex.Origin}");
				return (int)ExitCode.HtlmChanged;
			}
			catch (Exception ex)
            {
				Console.WriteLine($"Error occured : {ex.Message}");
				return (int)ExitCode.UnknownError;
			}
        }
	}
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BernardeSaturdayCakes
{
    public class MySecretOptions
    {
        public UserSecrets UserSecrets { get; set; }
        public EmailConfig EmailConfig { get; set; }
    }

    public class UserSecrets
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    public class EmailConfig
    {
        public string GmailAccountName { get; set; }
        public string GmailAccountAddress { get; set; }
        public string GmailAdditionnalToName { get; set; }
        public string GmailAdditionnalToAddress { get; set; }
    }

    public static class ServiceProviderBuilder
    {
        public static IServiceProvider GetServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets(typeof(Program).Assembly)
                .Build();
            var services = new ServiceCollection();

            services.Configure<MySecretOptions>(configuration.GetSection("MySecretOptions"));

            var provider = services.BuildServiceProvider();
            return provider;
        }
    }
}

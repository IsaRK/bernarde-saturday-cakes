using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Util;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using System.IO;
using System;

namespace BernardeSaturdayCakes
{
    public class MessageManager
    {
		private const string emailSubjectSetting = "emailSubject";
		private const string gmailSmtpServerSetting = "gmailSmtpServer";

		private IOptions<MySecretOptions> myOptions;

        public MessageManager(IOptions<MySecretOptions> options)
        {
			myOptions = options;
		}

		public string GetTemplateBody()
        {
			var path = Path.Combine(Environment.CurrentDirectory, "EmailTemplate.html");
			return File.ReadAllText(path);
		}

		public MimeMessage CreateMessage(string body)
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress(myOptions.Value.EmailConfig.GmailAccountName, myOptions.Value.EmailConfig.GmailAccountAddress));
			message.To.Add(new MailboxAddress(myOptions.Value.EmailConfig.GmailAccountName, myOptions.Value.EmailConfig.GmailAccountAddress));
			message.To.Add(new MailboxAddress(myOptions.Value.EmailConfig.GmailAdditionnalToName, myOptions.Value.EmailConfig.GmailAdditionnalToAddress));
			message.Subject = ConfigurationManager.AppSettings[emailSubjectSetting];

			var bodyBuilder = new BodyBuilder();
			bodyBuilder.HtmlBody = body;
			message.Body = bodyBuilder.ToMessageBody();

			return message;
		}

		private UserCredential GetCredentials()
		{
			var clientSecrets = new ClientSecrets
			{
				ClientId = myOptions.Value.UserSecrets.ClientId,
				ClientSecret = myOptions.Value.UserSecrets.ClientSecret,
			};

			return GoogleWebAuthorizationBroker.AuthorizeAsync(
				clientSecrets,
				new[] { GmailService.Scope.MailGoogleCom },
				myOptions.Value.EmailConfig.GmailAccountAddress,
			CancellationToken.None).Result;
		}

		public async Task SendEmail(MimeMessage mimeMessage)
		{
			var googleCredentials = GetCredentials();

			if (googleCredentials.Token.IsExpired(SystemClock.Default))
			{
				await googleCredentials.RefreshTokenAsync(CancellationToken.None);
			}

			using (var client = new SmtpClient())
			{
				client.Connect(ConfigurationManager.AppSettings[gmailSmtpServerSetting], 587, SecureSocketOptions.StartTls);

				var oauth2 = new SaslMechanismOAuth2(googleCredentials.UserId, googleCredentials.Token.AccessToken);
				client.Authenticate(oauth2);
				client.Send(mimeMessage);
				client.Disconnect(true);
			}
		}
	}
}

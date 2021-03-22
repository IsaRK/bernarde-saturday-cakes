# Bernarde Saturday Cakes

## Backstory

My local (but famous) bakery makes some of the best cakes ever.

A new unique cake is made every Saturday. The cake description is published on the bakery's website the monday before.
Unfortunately, they don't have a newsletter or way to notify people of this new cake.

## App Features

This small console app will :

- crawl the website and retrieve info about the new saturday "pâtisserie" (and the cake of the week)
- send me an email with all the infos

This a small project. I don't plan to deploy it on the cloud. I will keep it locally and it will be launch with a task scheduler.

## Tech I used/learned

In this project, there are examples of:

- Html Agility Pack usage to load an HTML Page
- XPath syntax for DOM manipulation and traversing
- Html Mail Body formatting with mjml
- Mail sending with the MailKit SmtpClient
- .NET Secret storage using the Configuration API

## OAuth 2.0 flow

I used a 3-legged flow OAuth 2.0 flow - not a [service account](https://developers.google.com/identity/protocols/oauth2/service-account]) or [2-legged OAuth](https://stackoverflow.com/questions/5593348/difference-between-oauth-2-0-two-legged-and-three-legged-implementation).

I choosed this flow because I'm making an API call on behalf of a user (me). So I need to delegate my permission to my application.
For this, I used the [Google OAuth2 API](https://developers.google.com/gmail/api/quickstart/dotnet).

I also could not use the Service account with [domain wide delegation](https://developers.google.com/identity/protocols/oauth2/service-account#delegatingauthority) because I'm not a super-administrator of the G suite domain.

The app was registered using the Google Console API on the Google Cloud Platform.

![](https://www.google.com/support/enterprise/static/gapps/art/admin/en/cpanel/3-legged-oauth-diagram.png)

## Secret Storage

The [Microsoft Doc](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-5.0&tabs=windows) only documents the secret storage for ASP .NET applications. I had to adapt this to Console App by creating a ConfigurationBuilder - I followed this [tutorial](https://www.frakkingsweet.com/add-usersecrets-to-net-core-console-application/).

## Credits

[Html Agility Pack](https://html-agility-pack.net/)

[MimeKit and MailKit Library](http://www.mimekit.net/)

[mjml Mail Formatting](https://mjml.io/)

## Contribute

This project is not open to contribution as it's a very small personnal project. But you are welcome to take a look/copy/fork any part of it !

## License

MIT © [IsaRK](https://github.com/IsaRK)

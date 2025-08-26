using Helpers.Mail;
using Helpers.Mail.Abstractions;
using Helpers.Mail.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Demo.Mail;

internal class Program
{
    static async Task Main(string[] args)
    {
        var services = ConfigureServices();
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;
        
        var mailSender = provider.GetRequiredService<IMailSender>();

        byte[] data = File.ReadAllBytes("cat.jpg");

        var mailBuilder = new MailBuilder()
            .From("John Doe", "jdoe@test.com")
            .To("Chester Westminster", "cwestminster@doodle.com")
            .To("Jane Doe", "jdoe@test.com")
            .To("", "rieger@test.com")
            .WithSubject("This is a Test Email")
            .WithContentResource("cat.jpg", data, out string cid_catImage)
            .WithHtmlBody($"""
                <h1>This is a Html Content Email</h1>
                <p>
                    This is a test email with an image attachment.</p>
                <img width="200" src="cid:{cid_catImage}" />
                <a href="https://google.com">Click here</a> to visit our website.
            """)
            .WithTextBody("This is a text-only content");

        var message = mailBuilder.Build();

        await mailSender.SendAsync(message);
    }

    private static ServiceProvider ConfigureServices()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var services = new ServiceCollection();

        services.AddSmtpMailSender(config);
        services.AddLogging(logConfig =>
        {
            logConfig.AddConsole();
            logConfig.AddSimpleConsole(options =>
            {
                options.IncludeScopes = false;
                options.SingleLine = false;
                options.TimestampFormat = "[HH:mm:ss] ";
            });
        });

        return services.BuildServiceProvider();
    }

}

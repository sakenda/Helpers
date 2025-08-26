using Helpers.Mail.Abstractions;
using Helpers.Mail.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Helpers.Mail.Extensions;

/// <summary>
/// Provides extension methods for registering mail sender services in an <see cref="IServiceCollection"/>.
/// </summary>
/// <remarks>This class contains methods to simplify the registration of mail sender services, such as configuring
/// and validating <see cref="MailSettings"/> and registering implementations of <see cref="IMailSender"/>.</remarks>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the SMTP mail sender service to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <remarks>This method configures the <see cref="MailSettings"/> options by binding them to the
    /// corresponding configuration section and validates that the SMTP server is specified. It registers the <see
    /// cref="SmtpMailSender"/> implementation of  <see cref="IMailSender"/> as a singleton service.</remarks>
    /// <param name="services">The <see cref="IServiceCollection"/> to which the mail sender service will be added.</param>
    /// <param name="configuration">The application's configuration, used to bind the <see cref="MailSettings"/> section.</param>
    /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
    public static IServiceCollection AddSmtpMailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<MailSettings>()
            .Bind(configuration.GetSection(nameof(MailSettings)))
            .Validate(s => !string.IsNullOrWhiteSpace(s.SmtpServer), "SMTP server must not be empty")
            .ValidateOnStart();

        services.AddSingleton<IMailSender, SmtpMailSender>();

        return services;
    }

}

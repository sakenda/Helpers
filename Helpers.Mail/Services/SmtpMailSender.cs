using Helpers.Mail.Abstractions;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Helpers.Mail.Services;

public class SmtpMailSender : IMailSender
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly bool _useSsl;
    private readonly string? _username;
    private readonly string? _password;
    private readonly ILogger<SmtpMailSender>? _logger;

    public SmtpMailSender(IOptions<MailSettings> options, ILogger<SmtpMailSender>? logger = null)
    {
        _smtpServer = options.Value.SmtpServer;
        _port = options.Value.Port;
        _useSsl = options.Value.UseSsl;
        _username = options.Value.Username;
        _password = options.Value.Password;
        _logger = logger;
    }

    public async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_smtpServer, _port, _useSsl).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(_username) && !string.IsNullOrWhiteSpace(_password))
                await client.AuthenticateAsync(_username, _password).ConfigureAwait(false);

            await client.SendAsync(message).ConfigureAwait(false);

            _logger?.LogInformation("Email sent successfully to {Recipients}", string.Join(", ", message.To.Select(x => x.Name)));
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error sending email");
            throw;
        }
        finally
        {
            await client.DisconnectAsync(true).ConfigureAwait(false);
        }
    }
}

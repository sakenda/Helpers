using MimeKit;

namespace Helpers.Mail.Abstractions;

public interface IMailSender
{
    /// <summary>
    /// Sends the specified email message asynchronously.
    /// </summary>
    /// <param name="message">The email message to be sent. Cannot be <see langword="null"/>.</param>
    /// <returns>A task that represents the asynchronous send operation.</returns>
    Task SendAsync(MimeMessage message);
}

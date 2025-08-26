using MimeKit;
using MimeKit.Utils;
using Helpers.Mail.Services;

namespace Helpers.Mail;

/// <summary>
/// Provides a fluent interface for constructing email messages.
/// </summary>
/// <remarks>
/// This class allows you to build an email message step by step, setting properties such as sender, recipients, subject, body content, and attachments.<br />
/// The returned <see cref="MimeMessage"/> can be used with any SMTP client that supports the MimeKit library, such as <see cref="SmtpMailSender"/>.<br />
/// </remarks>
public class MailBuilder
{
    private readonly BodyBuilder _bodyBuilder = new();
    private MailboxAddress? _from;
    private readonly List<MailboxAddress> _to = new();
    private readonly List<MailboxAddress> _cc = new();
    private readonly List<MailboxAddress> _bcc = new();
    private string _subject = string.Empty;

    /// <summary>
    /// Sets the sender's name and email address for the email being constructed.
    /// </summary>
    /// <param name="name">The display name of the sender. This can be null or empty if no display name is required.</param>
    /// <param name="email">The email address of the sender. This cannot be null or empty.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance, allowing for method chaining.</returns>
    public MailBuilder From(string name, string email)
    {
        _from = new MailboxAddress(name, email);
        return this;
    }

    /// <summary>
    /// Adds a recipient to the email with the specified name and email address.
    /// </summary>
    /// <param name="name">The display name of the recipient. This can be null or empty if no display name is required.</param>
    /// <param name="email">The email address of the recipient. This cannot be null or empty.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance, allowing for method chaining.</returns>
    public MailBuilder To(string name, string email)
    {
        _to.Add(new MailboxAddress(name, email));
        return this;
    }

    /// <summary>
    /// Adds a recipient to the CC (carbon copy) list of the email.
    /// </summary>
    /// <param name="name">The display name of the recipient.</param>
    /// <param name="email">The email address of the recipient. Must be a valid email address.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance, allowing for method chaining.</returns>
    public MailBuilder Cc(string name, string email)
    {
        _cc.Add(new MailboxAddress(name, email));
        return this;
    }

    /// <summary>
    /// Adds a recipient to the Bcc (blind carbon copy) list for the email being constructed.
    /// </summary>
    /// <param name="name">The display name of the recipient.</param>
    /// <param name="email">The email address of the recipient. Must be a valid email address.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance, allowing for method chaining.</returns>
    public MailBuilder Bcc(string name, string email)
    {
        _bcc.Add(new MailboxAddress(name, email));
        return this;
    }

    /// <summary>
    /// Sets the subject of the email and returns the updated <see cref="MailBuilder"/> instance.
    /// </summary>
    /// <param name="subject">The subject line of the email. Cannot be null or empty.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance with the updated subject.</returns>
    public MailBuilder WithSubject(string subject)
    {
        _subject = subject;
        return this;
    }

    /// <summary>
    /// Sets the plain text body of the email message.
    /// </summary>
    /// <param name="text">The plain text content to include in the email body. Cannot be null or empty.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance, allowing for method chaining.</returns>
    public MailBuilder WithTextBody(string text)
    {
        _bodyBuilder.TextBody = text;
        return this;
    }

    /// <summary>
    /// Sets the HTML content of the email body.
    /// </summary>
    /// <param name="html">The HTML string to use as the email body. This value cannot be null or empty.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance, allowing for method chaining.</returns>
    public MailBuilder WithHtmlBody(string html)
    {
        _bodyBuilder.HtmlBody = html;
        return this;
    }

    /// <summary>
    /// Adds an attachment to the email being constructed.
    /// </summary>
    /// <remarks>Use this method to attach a file to the email. The attachment is identified by its filename
    /// and  contains the provided binary data. This method supports method chaining, enabling multiple  attachments to
    /// be added in a fluent manner.</remarks>
    /// <param name="filename">The name of the file to be attached. This value cannot be null or empty.</param>
    /// <param name="data">The binary content of the file to be attached. This value cannot be null.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance, allowing for method chaining.</returns>
    public MailBuilder WithAttachment(string filename, byte[] data)
    {
        _bodyBuilder.Attachments.Add(filename, data);
        return this;
    }
        
    /// <summary>
    /// Adds a content resource to the email and generates a unique content ID for it.
    /// </summary>
    /// <remarks>The content resource is added as a linked resource to the email body. The generated  content
    /// ID can be used to reference the resource within the email content.</remarks>
    /// <param name="filename">The name of the file to associate with the content resource.</param>
    /// <param name="data">The binary data of the content resource.</param>
    /// <param name="contentId">When this method returns, contains the unique content ID generated for the resource.  This parameter is passed
    /// uninitialized.</param>
    /// <returns>The current <see cref="MailBuilder"/> instance, allowing for method chaining.</returns>
    public MailBuilder WithContentResource(string filename, byte[] data, out string contentId)
    {
        var entity = _bodyBuilder.LinkedResources.Add(filename, data);
        entity.ContentId = MimeUtils.GenerateMessageId();
        contentId = entity.ContentId;
        return this;
    }

    /// <summary>
    /// Builds and returns a complete <see cref="MimeMessage"/> instance based on the configured sender, recipients,
    /// subject, and body.
    /// </summary>
    /// <remarks>This method requires that a sender (From) and at least one recipient (To) are set before
    /// calling it.  If these conditions are not met, an <see cref="InvalidOperationException"/> is thrown.</remarks>
    /// <returns>A <see cref="MimeMessage"/> instance containing the configured sender, recipients, subject, and body.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the sender (From) is not set or if no recipients (To) are specified.</exception>
    public MimeMessage Build()
    {
        if (_from == null) throw new InvalidOperationException("Sender (From) must be set.");
        if (!_to.Any()) throw new InvalidOperationException("At least one recipient must be set.");

        var message = new MimeMessage();
        message.From.Add(_from);
        message.To.AddRange(_to);
        message.Cc.AddRange(_cc);
        message.Bcc.AddRange(_bcc);
        message.Subject = _subject;
        message.Body = _bodyBuilder.ToMessageBody();

        return message;
    }

}

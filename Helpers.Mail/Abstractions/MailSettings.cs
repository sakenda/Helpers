using System.ComponentModel.DataAnnotations;

namespace Helpers.Mail.Abstractions;

public class MailSettings
{
    [Required]
    public string SmtpServer { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public bool UseSsl { get; set; } = true;
    public string? Username { get; set; }
    public string? Password { get; set; }

}

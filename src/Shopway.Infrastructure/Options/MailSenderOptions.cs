namespace Shopway.Infrastructure.Options;

public sealed class MailSenderOptions
{
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; }
    public bool UseSSL { get; set; }
}

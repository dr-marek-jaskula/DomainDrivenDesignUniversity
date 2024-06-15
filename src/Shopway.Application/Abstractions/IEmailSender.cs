namespace Shopway.Application.Abstractions;

public interface IEmailSender
{
    Task SendEmailAsync(string toName, string toAddress, string fromAddress, string subject, string body, CancellationToken cancellationToken);
}

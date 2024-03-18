namespace Shopway.Application.Abstractions;

public interface ISendEmail
{
    Task SendEmailAsync(string toName, string toAddress, string fromAddress, string subject, string body, CancellationToken cancellationToken);
}
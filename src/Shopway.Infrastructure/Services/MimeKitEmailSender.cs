using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Shopway.Application.Abstractions;
using Shopway.Infrastructure.Options;

namespace Shopway.Infrastructure.Services;

public sealed class MimeKitEmailSender(ILogger<MimeKitEmailSender> logger, IOptions<MailSenderOptions> options) : IEmailSender
{
    private readonly ILogger<MimeKitEmailSender> _logger = logger;
    private readonly MailSenderOptions _options = options.Value;

    public async Task SendEmailAsync(string toName, string toAddress, string fromAddress, string subject, string body, CancellationToken cancellationToken)
    {
        _logger.LogSendingEmail(toAddress, fromAddress, subject);

        using var client = new SmtpClient();

        try
        {
            client.Connect(_options.Host, _options.Port, _options.UseSSL);

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameof(Shopway), fromAddress));
            message.To.Add(new MailboxAddress(toName, toAddress));
            message.Subject = subject;
            message.Body = new TextPart() { Text = body };

            var response = await client.SendAsync(message, cancellationToken);
            _logger.LogEmailSent(toAddress, fromAddress, subject, response);
        }
        catch (Exception exception)
        {
            _logger.LogEmailNotSent(toAddress, fromAddress, subject, exception.Message);
            throw;
        }
        finally
        {
            client.Disconnect(true);
        }
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 1,
        EventName = $"{nameof(MimeKitEmailSender)}",
        Level = LogLevel.Information,
        Message = "Attempting to send email from {from} to {to} with subject {subject}",
        SkipEnabledCheck = false
    )]
    public static partial void LogSendingEmail(this ILogger logger, string to, string from, string subject);
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 2,
        EventName = $"{nameof(MimeKitEmailSender)}",
        Level = LogLevel.Information,
        Message = "Email from {from} to {to} with subject {subject} sent. Server response {serverResponse}",
        SkipEnabledCheck = false
    )]
    public static partial void LogEmailSent(this ILogger logger, string to, string from, string subject, string serverResponse);
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 3,
        EventName = $"{nameof(MimeKitEmailSender)}",
        Level = LogLevel.Error,
        Message = "Email from {from} to {to} with subject {subject} failed to sent. Reason: {exceptionMessage}",
        SkipEnabledCheck = false
    )]
    public static partial void LogEmailNotSent(this ILogger logger, string to, string from, string subject, string exceptionMessage);
}
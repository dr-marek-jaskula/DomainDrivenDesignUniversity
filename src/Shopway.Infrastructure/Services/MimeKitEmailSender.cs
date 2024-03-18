using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Shopway.Application.Abstractions;
using Shopway.Infrastructure.Options;

namespace Shopway.Infrastructure.Services;

public sealed class MimeKitEmailSender(ILogger<MimeKitEmailSender> logger, IOptions<MailSenderOptions> options) : ISendEmail
{
    private readonly ILogger<MimeKitEmailSender> _logger = logger;
    private readonly MailSenderOptions _options = options.Value;

    public async Task SendEmailAsync(string toName, string toAddress, string fromAddress, string subject, string body, CancellationToken cancellationToken)
    {
        _logger.LogSendingEmail(toAddress, fromAddress, subject);

        using var client = new SmtpClient();

        client.Connect(_options.Host, _options.Port, _options.UseSSL);

        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(nameof(Shopway), fromAddress));
        message.To.Add(new MailboxAddress(toName, toAddress));
        message.Subject = subject;
        message.Body = new TextPart() { Text = body };

        await client.SendAsync(message, cancellationToken);

        _logger.LogEmailSent(toAddress, fromAddress, subject);

        client.Disconnect(true);
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
    public static partial void LogSendingEmail(this ILogger logger, string from, string to, string subject);
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 1,
        EventName = $"{nameof(MimeKitEmailSender)}",
        Level = LogLevel.Information,
        Message = "Email from {from} to {to} with subject {subject} sent",
        SkipEnabledCheck = false
    )]
    public static partial void LogEmailSent(this ILogger logger, string from, string to, string subject);
}
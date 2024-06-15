using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shopway.Application.Abstractions;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Events;

namespace Shopway.Application.Features.Users.Events;

internal sealed class TwoFactorTokenCreatedDomainEventHandler
(
    IPasswordHasher<User> passwordHasher,
    IUserRepository userRepository,
    ILogger<TwoFactorTokenCreatedDomainEventHandler> logger,
    IEmailSender sendEmail
)
    : IDomainEventHandler<TwoFactorTokenCreatedDomainEvent>
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ILogger<TwoFactorTokenCreatedDomainEventHandler> _logger = logger;
    private readonly IEmailSender _sendEmail = sendEmail;

    public async Task Handle(TwoFactorTokenCreatedDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(domainEvent.UserId, cancellationToken);

        if (user is null || user.TwoFactorTokenHash is null)
        {
            _logger.LogUserOrTwoFactorTokenNotFound(domainEvent.UserId);
            return;
        }

        var result = _passwordHasher
            .VerifyHashedPassword(user!, user!.TwoFactorTokenHash!.Value, domainEvent.TwoFactorToken);

        if (result is not PasswordVerificationResult.Success)
        {
            _logger.LogTwoFactorTokenDoesNotMatchCurrentHash(domainEvent.UserId);
            return;
        }

        await _sendEmail.SendEmailAsync
        (
            user.Username.Value,
            user.Email.Value,
            $"{nameof(Shopway)}@test.com",
            $"{nameof(Shopway)} Authorization Code",
            $"Hello {user.Username.Value}, \n \n thank You for using the {nameof(Shopway)}! \n \n Here is the authorization code. It is valid only for a short period of time. After failure attempt, code is reseted. \n \n Code: {domainEvent.TwoFactorToken}",
            cancellationToken
        );
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 1,
        EventName = $"{nameof(TwoFactorTokenCreatedDomainEventHandler)}",
        Level = LogLevel.Warning,
        Message = "User with id {userId} was not found or its TwoFactorToken was null",
        SkipEnabledCheck = false
    )]
    public static partial void LogUserOrTwoFactorTokenNotFound(this ILogger logger, UserId userId);

    [LoggerMessage
    (
        EventId = 2,
        EventName = $"{nameof(TwoFactorTokenCreatedDomainEventHandler)}_HashMatch",
        Level = LogLevel.Warning,
        Message = "User with id {userId} has TwoFactorTokenHash that differs processed TwoFactorToken",
        SkipEnabledCheck = false
    )]
    public static partial void LogTwoFactorTokenDoesNotMatchCurrentHash(this ILogger logger, UserId userId);
}

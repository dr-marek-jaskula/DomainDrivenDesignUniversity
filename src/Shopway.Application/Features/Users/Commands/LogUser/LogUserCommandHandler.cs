using Microsoft.AspNetCore.Identity;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;
using static Shopway.Domain.Users.Errors.DomainErrors.PasswordOrEmailError;

namespace Shopway.Application.Features.Users.Commands.LogUser;

internal sealed class LogUserCommandHandler
(
    IUserRepository userRepository,
    ISecurityTokenService securityTokenService,
    IValidator validator,
    IPasswordHasher<User> passwordHasher
)
    : ICommandHandler<LogUserCommand, AccessTokenResponse>
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISecurityTokenService _securityTokenService = securityTokenService;
    private readonly IValidator _validator = validator;

    public async Task<IResult<AccessTokenResponse>> Handle(LogUserCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<Email> emailResult = Email.Create(command.Email);
        ValidationResult<Password> passwordResult = Password.Create(command.Password);

        _validator
            .Validate(emailResult)
            .Validate(passwordResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        User? user = await _userRepository
            .GetByEmailAsync(emailResult.Value, cancellationToken);

        _validator
            .If(user is null, thenError: InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var result = _passwordHasher
            .VerifyHashedPassword(user!, user!.PasswordHash.Value, passwordResult.Value.Value);

        _validator
            .If(result is PasswordVerificationResult.Failed, thenError: InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var accessTokenResult = _securityTokenService.GenerateJwt(user);

        var refreshTokenResult = RefreshToken.Create(accessTokenResult.RefreshToken);

        _validator
            .Validate(refreshTokenResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        user.RefreshToken = refreshTokenResult.Value;

        return accessTokenResult
            .ToResult();
    }
}

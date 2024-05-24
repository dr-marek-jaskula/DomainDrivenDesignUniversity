using Microsoft.AspNetCore.Identity;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;
using static Shopway.Domain.Users.Errors.DomainErrors.PasswordOrEmailError;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorTopt;

internal sealed class LoginTwoFactorToptCommandHandler
(
    IUserRepository userRepository,
    IValidator validator,
    ISecurityTokenService securityTokenService,
    IPasswordHasher<User> passwordHasher,
    IToptService toptService
)
    : ICommandHandler<LoginTwoFactorToptCommand, AccessTokenResponse>
{
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IToptService _toptService = toptService;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IValidator _validator = validator;
    private readonly ISecurityTokenService _securityTokenService = securityTokenService;

    public async Task<IResult<AccessTokenResponse>> Handle(LoginTwoFactorToptCommand command, CancellationToken cancellationToken)
    {
        var email = Email.Create(command.Email).Value;
        var password = Password.Create(command.Password).Value;

        User? user = await _userRepository
            .GetByEmailAsync(email, cancellationToken);

        _validator
            .If(user is null, thenError: InvalidPasswordOrEmail);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var result = _passwordHasher
            .VerifyHashedPassword(user!, user!.PasswordHash.Value, password.Value);

        _validator
            .If(result is PasswordVerificationResult.Failed, thenError: InvalidPasswordOrEmail)
            .If(user.TwoFactorToptSecret is null, Error.New(nameof(user.TwoFactorToptSecret), $"{nameof(user.TwoFactorToptSecret)} is not configured"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        _validator
            .If(_toptService.VerifyCode(user.TwoFactorToptSecret!.Value, command.Code) is false, thenError: Error.InvalidArgument($"{nameof(command.Code)} is invalid"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var accessTokenResponse = _securityTokenService.GenerateJwt(user);
        var refreshTokenResult = RefreshToken.Create(accessTokenResponse.RefreshToken);

        _validator
            .Validate(refreshTokenResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        user.Refresh(refreshTokenResult.Value);

        return accessTokenResponse
            .ToResult();
    }
}

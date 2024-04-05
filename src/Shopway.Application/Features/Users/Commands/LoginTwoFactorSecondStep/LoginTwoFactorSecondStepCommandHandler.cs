using Microsoft.AspNetCore.Identity;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorSecondStep;

internal sealed class LoginTwoFactorSecondStepCommandHandler
(
    IUserRepository userRepository,
    ISecurityTokenService securityTokenService,
    IValidator validator,
    IPasswordHasher<User> passwordHasher
)
    : ICommandHandler<LoginTwoFactorSecondStepCommand, AccessTokenResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISecurityTokenService _securityTokenService = securityTokenService;
    private readonly IValidator _validator = validator;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<IResult<AccessTokenResponse>> Handle(LoginTwoFactorSecondStepCommand command, CancellationToken cancellationToken)
    {
        ValidationResult<TwoFactorTokenHash> twoFactorTokenResult = TwoFactorTokenHash.Create(command.TwoFactorToken);
        ValidationResult<Email> emailResult = Email.Create(command.Email);

        _validator
            .Validate(twoFactorTokenResult)
            .Validate(emailResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        User? user = await _userRepository
            .GetByEmailAsync(emailResult.Value, cancellationToken);

        _validator
            .If(user?.TwoFactorTokenHash is null, thenError: Error.InvalidArgument("User not found or token is null"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var result = _passwordHasher
            .VerifyHashedPassword(user!, user!.TwoFactorTokenHash!.Value, command.TwoFactorToken);

        _validator
            .If(result is not PasswordVerificationResult.Success, thenError: Error.InvalidArgument("Invalid TwoFactorToken"))
            .If(_securityTokenService.HasTwoFactorTokenExpired(user!.TwoFactorTokenCreatedOn), thenError: Error.InvalidArgument("TwoFactorToken has expired"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var accessTokenResult = _securityTokenService.GenerateJwt(user!);

        var refreshTokenResult = RefreshToken.Create(accessTokenResult.RefreshToken);

        _validator
            .Validate(refreshTokenResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        user!.RefreshToken = refreshTokenResult.Value;
        user!.ClearTwoFactorToken();

        return accessTokenResult
            .ToResult();
    }
}

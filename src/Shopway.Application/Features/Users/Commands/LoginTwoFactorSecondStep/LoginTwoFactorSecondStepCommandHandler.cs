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
        var email = Email.Create(command.Email).Value;
        var password = Password.Create(command.Password).Value;

        User? user = await _userRepository
            .GetByEmailAsync(email, cancellationToken);

        _validator
            .If(user?.TwoFactorTokenHash is null, thenError: Error.InvalidArgument("User not found or token is null"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var passwordVerificationResult = _passwordHasher
            .VerifyHashedPassword(user!, user!.PasswordHash.Value, password.Value);

        var twoFactorTokenVerificaitonResult = _passwordHasher
            .VerifyHashedPassword(user!, user!.TwoFactorTokenHash!.Value, command.TwoFactorToken);

        _validator
            .If(passwordVerificationResult is PasswordVerificationResult.Failed, thenError: Error.InvalidArgument("Invalid Password"))
            .If(twoFactorTokenVerificaitonResult is PasswordVerificationResult.Failed, thenError: Error.InvalidArgument("Invalid TwoFactorToken"))
            .If(_securityTokenService.HasTwoFactorTokenExpired(user!.TwoFactorTokenCreatedOn), thenError: Error.InvalidArgument("TwoFactorToken has expired"));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var accessTokenResponse = _securityTokenService.GenerateJwt(user!);
        var refreshTokenResult = RefreshToken.Create(accessTokenResponse.RefreshToken);

        _validator
            .Validate(refreshTokenResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        user.Refresh(refreshTokenResult.Value);
        user.ClearTwoFactorToken();

        return accessTokenResponse
            .ToResult();
    }
}

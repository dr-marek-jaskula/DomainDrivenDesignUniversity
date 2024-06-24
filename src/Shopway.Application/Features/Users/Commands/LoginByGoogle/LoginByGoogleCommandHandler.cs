using Microsoft.AspNetCore.Identity;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.Authorization;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.LoginByGoogle;

internal sealed class LoginByGoogleCommandHandler
(
    IUserRepository userRepository,
    ISecurityTokenService securityTokenService,
    IValidator validator,
    IPasswordHasher<User> passwordHasher,
    IAuthorizationRepository authorizationRepository
)
    : ICommandHandler<LoginByGoogleCommand, AccessTokenResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISecurityTokenService _securityTokenService = securityTokenService;
    private readonly IValidator _validator = validator;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IAuthorizationRepository _authorizationRepository = authorizationRepository;
    private readonly RoleName _customerRoleName = RoleName.Customer;

    public async Task<IResult<AccessTokenResponse>> Handle(LoginByGoogleCommand command, CancellationToken cancellationToken)
    {
        var userLogDetailsResult = _securityTokenService.GetUserLogDetailsFormGoogleClaims(command.ClaimsPrincipal);

        _validator
            .Validate(userLogDetailsResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        (Email email, Username username) = userLogDetailsResult.Value;

        var user = await _userRepository.GetByEmailAsync(email, cancellationToken);

        if (user is null)
        {
            var userResult = await RegisterGoogleUser(email, username, cancellationToken);

            _validator
                .Validate(userResult);

            if (_validator.IsInvalid)
            {
                return _validator.Failure<AccessTokenResponse>();
            }

            user = userResult.Value;
        }

        var accessTokenResponse = _securityTokenService.GenerateJwt(user!);
        var refreshTokenResult = RefreshToken.Create(accessTokenResponse.RefreshToken);

        _validator
            .Validate(refreshTokenResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        user!.Refresh(refreshTokenResult.Value);

        return accessTokenResponse
            .ToResult();
    }

    private async Task<Result<User>> RegisterGoogleUser(Email email, Username username, CancellationToken cancellationToken)
    {
        var user = User.New(username, email);

        var randomPasswordResult = Password.CreateRandomPassword();

        if (randomPasswordResult.IsFailure)
        {
            return randomPasswordResult.Failure<User>();
        }

        var passwordHashResult = PasswordHash.Create(_passwordHasher.HashPassword(user, randomPasswordResult.Value.Value));

        if (passwordHashResult.IsFailure)
        {
            return passwordHashResult.Failure<User>();
        }

        user.SetHashedPassword(passwordHashResult.Value);

        var externalIdentityProviderResult = ExternalIdentityProvider.Create(ExternalIdentityProvider.GoogleExternalIdentityProvider);

        if (externalIdentityProviderResult.IsFailure)
        {
            return externalIdentityProviderResult.Failure<User>();
        }

        user.SetExternalIdentityProvider(externalIdentityProviderResult.Value);

        var role = await _authorizationRepository
            .GetRolePermissionsAsync(_customerRoleName, cancellationToken);

        if (role is null)
        {
            Result.Failure<User>(Error.NotFound(nameof(Role), $"{_customerRoleName}", "RoleName does not match role in database"));
        }

        if (user.Roles.Any(x => x.Name == role?.Name))
        {
            Result.Failure<User>(Error.AlreadyExists(nameof(Role), $"{role?.Name}"));
        }

        user.AddRole(role!);
        _userRepository.Add(user);
        return Result.Success(user);
    }
}

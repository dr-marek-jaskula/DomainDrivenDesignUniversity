using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Users;
using Shopway.Domain.Users.ValueObjects;

namespace Shopway.Application.Features.Users.Commands.RefreshAccessToken;

internal sealed class RefreshAccessTokenCommandHandler
(
    IUserRepository userRepository,
    ISecurityTokenService securityTokenService,
    IValidator validator
)
    : ICommandHandler<RefreshAccessTokenCommand, AccessTokenResponse>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ISecurityTokenService _securityTokenService = securityTokenService;
    private readonly IValidator _validator = validator;

    public async Task<IResult<AccessTokenResponse>> Handle(RefreshAccessTokenCommand command, CancellationToken cancellationToken)
    {
        var hasRefreshTokenExpiredResult = _securityTokenService.HasRefreshTokenExpired(command.AccessToken);

        _validator
            .Validate(hasRefreshTokenExpiredResult);

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var emailClaim = _securityTokenService.GetClaimFromToken(command.AccessToken, nameof(Email)).Value;
        var email = Email.Create(emailClaim!.Value).Value;

        User? user = await _userRepository
            .GetByEmailAsync(email, cancellationToken);

        _validator
            .If(user is null, thenError: Error.NotFound<User>(email.Value));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<AccessTokenResponse>();
        }

        var refreshTokenFromCommand = RefreshToken.Create(command.RefreshToken).Value;

        _validator
            .If(user!.RefreshToken != refreshTokenFromCommand, thenError: RefreshToken.NotMatch);

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

        user!.Refresh(refreshTokenResult.Value);

        return accessTokenResponse
            .ToResult();
    }
}

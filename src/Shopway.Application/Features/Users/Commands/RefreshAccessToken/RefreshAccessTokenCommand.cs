using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.RefreshAccessToken;

public sealed record RefreshAccessTokenCommand
(
    string AccessToken,
    string RefreshToken
)
    : ICommand<AccessTokenResponse>;

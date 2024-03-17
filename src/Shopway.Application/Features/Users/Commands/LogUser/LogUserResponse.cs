using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Users.Commands.LogUser;

public sealed record LogUserResponse
(
    string AccessToken,
    int ExpiresInMinutes,
    string RefreshToken,
    string TokenType
) : IResponse;
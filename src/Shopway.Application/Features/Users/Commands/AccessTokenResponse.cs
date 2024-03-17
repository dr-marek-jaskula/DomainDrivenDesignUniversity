using Shopway.Application.Abstractions;
using static Shopway.Application.Features.Users.Commands.AccessTokenResponse;

namespace Shopway.Application.Features.Users.Commands;

public sealed record AccessTokenResponse(string AccessToken, int ExpiresInMinutes, string RefreshToken, string TokenType = Bearer) : IResponse
{
    public const string Bearer = nameof(Bearer);
}
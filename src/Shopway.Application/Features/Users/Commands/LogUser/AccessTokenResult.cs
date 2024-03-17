using static Shopway.Application.Features.Users.Commands.LogUser.AccessTokenResult;

namespace Shopway.Application.Features.Users.Commands.LogUser;

public sealed record AccessTokenResult(string AccessToken, int ExpiresInMinutes, string RefreshToken, string TokenType = Bearer)
{
    public const string Bearer = nameof(Bearer);
}
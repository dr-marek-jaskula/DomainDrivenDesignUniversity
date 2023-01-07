using Shopway.Application.Abstractions;

namespace Shopway.Application.CQRS.Users.Commands.LogUser;

public sealed record LogUserResponse
(
    string Token
) : IResponse;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.LogUser;

public sealed record LogUserCommand
(
    string Email,
    string Password
)
    : ICommand<AccessTokenResponse>;

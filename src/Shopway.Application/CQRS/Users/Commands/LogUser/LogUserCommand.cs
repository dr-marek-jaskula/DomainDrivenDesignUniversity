using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Users.Commands.LogUser;

public sealed record LogUserCommand
(
    string Email,
    string Password
)
    : ICommand<LogUserResponse>;
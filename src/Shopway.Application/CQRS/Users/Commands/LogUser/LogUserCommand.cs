using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.CQRS.Users.Commands.LogUser;

public sealed record LogUserCommand
(
    string Username,
    string Email
)
    : ICommand<LogUserResponse>;
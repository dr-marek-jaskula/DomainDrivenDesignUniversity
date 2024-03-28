using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorTopt;

public sealed record LoginTwoFactorToptCommand
(
    string Email,
    string Password,
    string Code
)
    : ICommand<AccessTokenResponse>;
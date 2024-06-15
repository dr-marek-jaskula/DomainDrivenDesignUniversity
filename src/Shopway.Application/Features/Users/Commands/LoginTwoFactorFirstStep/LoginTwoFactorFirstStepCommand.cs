using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorFirstStep;

public sealed record LoginTwoFactorFirstStepCommand
(
    string Email,
    string Password
)
    : ICommand;

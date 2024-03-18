using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorFirstPhase;

public sealed record LoginTwoFactorFirstPhaseCommand
(
    string Email,
    string Password
)
    : ICommand;
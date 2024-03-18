using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorSecondPhase;

public sealed record LoginTwoFactorSecondPhaseCommand
(
    string Email,
    string TwoFactorToken
) 
    : ICommand<AccessTokenResponse>;
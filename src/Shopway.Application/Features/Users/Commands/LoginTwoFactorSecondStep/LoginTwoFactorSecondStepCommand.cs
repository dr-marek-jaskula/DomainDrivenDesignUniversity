using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Users.Commands.LoginTwoFactorSecondStep;

public sealed record LoginTwoFactorSecondStepCommand
(
    string Email,
    string TwoFactorToken
) 
    : ICommand<AccessTokenResponse>;
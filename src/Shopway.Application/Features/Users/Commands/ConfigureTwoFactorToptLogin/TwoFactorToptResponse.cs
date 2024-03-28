using Shopway.Application.Abstractions;

namespace Shopway.Application.Features.Users.Commands.ConfigureTwoFactorToptLogin;

public sealed record TwoFactorToptResponse
(
    string QrCode
) : IResponse;

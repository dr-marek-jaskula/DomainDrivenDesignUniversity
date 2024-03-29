using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Commands.FinalizePaymentProcess;

public sealed record FinalizePaymentProcessCommand
(
    string SessionId,
    bool WasPaymentSuccessful,
    string SecretHash
) : ICommand;
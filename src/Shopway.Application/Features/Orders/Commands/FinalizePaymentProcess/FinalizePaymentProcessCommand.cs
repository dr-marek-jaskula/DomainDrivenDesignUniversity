using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Commands.FinalizePaymentProcess;

public sealed record FinalizePaymentProcessCommand : ICommand
{
    public static readonly FinalizePaymentProcessCommand Instance = new();
}

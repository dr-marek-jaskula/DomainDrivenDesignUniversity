using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Commands.CancelPaymentProcess;

public sealed record CancelPaymentProcessCommand : ICommand
{
    public static readonly CancelPaymentProcessCommand Instance = new();
}
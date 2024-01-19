using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;
using Shopway.Domain.Products;
using static Shopway.Application.Features.Orders.Commands.AddOrderLine.AddOrderLineCommand;

namespace Shopway.Application.Features.Orders.Commands.AddOrderLine;

public sealed record AddOrderLineCommand
(
    OrderHeaderId OrderHeaderId,
    ProductId ProductId,
    AddOrderLineRequestBody Body
) : ICommand<AddOrderLineResponse>
{
    public sealed record AddOrderLineRequestBody(int Amount, decimal? Discount);
}
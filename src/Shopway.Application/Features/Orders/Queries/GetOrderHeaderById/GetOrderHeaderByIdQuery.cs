using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderHeaderByIdQuery(OrderHeaderId Id) : IQuery<OrderHeaderResponse>;

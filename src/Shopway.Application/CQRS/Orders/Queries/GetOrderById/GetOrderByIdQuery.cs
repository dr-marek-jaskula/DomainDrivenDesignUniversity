using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.EntityIds;

namespace Shopway.Application.CQRS.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(OrderId Id) : IQuery<OrderResponse>;
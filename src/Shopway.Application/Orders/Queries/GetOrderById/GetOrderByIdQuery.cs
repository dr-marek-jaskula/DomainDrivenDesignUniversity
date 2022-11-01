using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(Guid Id) : IQuery<OrderResponse>;
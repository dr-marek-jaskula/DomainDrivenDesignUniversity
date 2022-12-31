using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Application.CQRS.Orders.Queries.GetOrderById;

public sealed record GetOrderByIdQuery(OrderId Id) : IQuery<OrderResponse>;
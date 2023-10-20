using Shopway.Domain.EntityIds;
using Shopway.Application.Abstractions.CQRS;

namespace Shopway.Application.Features.Orders.Queries.GetOrderById;

public sealed record GetOrderHeaderByIdQuery(OrderHeaderId Id) : IQuery<OrderHeaderResponse>;
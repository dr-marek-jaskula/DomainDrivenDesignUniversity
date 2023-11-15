﻿using Shopway.Domain.Abstractions;
using Shopway.Application.Mappings;
using Shopway.Application.Utilities;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Abstractions.Repositories;

namespace Shopway.Application.Features.Orders.Queries.GetOrderById;

internal sealed class GetOrderHeaderByIdQueryHandler(IOrderHeaderRepository orderRepository) 
    : IQueryHandler<GetOrderHeaderByIdQuery, OrderHeaderResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderRepository;

    public async Task<IResult<OrderHeaderResponse>> Handle(GetOrderHeaderByIdQuery query, CancellationToken cancellationToken)
    {
        var orderHeader = await _orderHeaderRepository
            .GetByIdAsync(query.Id, cancellationToken);

        return orderHeader
            .ToResponse()
            .ToResult();
    }
}
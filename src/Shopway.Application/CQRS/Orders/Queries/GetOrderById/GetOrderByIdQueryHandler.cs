using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Repositories;
using Shopway.Domain.Results;
using Shopway.Application.Mapping;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.CQRS.Orders.Queries.GetOrderById;

internal sealed class GetOrderByIdQueryHandler : IQueryHandler<GetOrderByIdQuery, OrderResponse>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IValidator _validator;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository, IValidator validator)
    {
        _orderRepository = orderRepository;
        _validator = validator;
    }

    public async Task<IResult<OrderResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(query.Id, cancellationToken);

        _validator
            .If(order is null, thenError: NotFound(nameof(Order), query.Id));

        if (_validator.IsInvalid)
        {
            return _validator.Failure<OrderResponse>();
        }

        var response = order!.ToResponse();

        return Result.Create(response);
    }
}
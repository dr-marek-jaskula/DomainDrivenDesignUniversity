using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Utilities;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Orders;

namespace Shopway.Application.Features.Orders.Queries;

internal sealed class OrderHeaderWithMappingQueryHandler(IOrderHeaderRepository orderHeaderRepository)
    : IQueryHandler<OrderHeaderWithMappingQuery, DataTransferObjectResponse>
{
    private readonly IOrderHeaderRepository _orderHeaderRepository = orderHeaderRepository;

    public async Task<IResult<DataTransferObjectResponse>> Handle(OrderHeaderWithMappingQuery query, CancellationToken cancellationToken)
    {
        var orderHeaderDto = await _orderHeaderRepository
            .QueryByIdAsync(query.OrderHeaderId, cancellationToken, query.Mapping);

        return orderHeaderDto
            .ToResponse()
            .ToResult();
    }
}

using MediatR;
using Shopway.Domain.Common.Results;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

public sealed class CreateOrderHeaderOpenTelemetryPipeline(CreateOrderHeaderOpenTelemetry orderMetrics) 
    : IPipelineBehavior<CreateOrderHeaderCommand, IResult<CreateOrderHeaderResponse>>
{
    private readonly CreateOrderHeaderOpenTelemetry _orderMetrics = orderMetrics;

    public async Task<IResult<CreateOrderHeaderResponse>> Handle(CreateOrderHeaderCommand command, RequestHandlerDelegate<IResult<CreateOrderHeaderResponse>> next, CancellationToken cancellationToken)
    {
        using var _ = _orderMetrics.MeasureRequestDuration();

        var result = await next();

        if (result.IsSuccess)
        {
            _orderMetrics.IncreaseOrderRequestCount(result.Value.Id);
        }

        return result;
    }
}

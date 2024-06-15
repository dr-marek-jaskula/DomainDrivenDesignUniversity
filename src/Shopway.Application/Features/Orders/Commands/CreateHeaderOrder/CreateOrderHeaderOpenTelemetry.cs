using Shopway.Application.OpenTelemetry;
using Shopway.Domain.Common.BaseTypes;
using System.Diagnostics.Metrics;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

public sealed class CreateOrderHeaderOpenTelemetry : OpenTelemetryBase
{
    private const int IncreaseCount = 1;
    private const string OrdersPrefix = $"{ShopwayApplicationPrefix}.orders";

    private const string OrderCounterName = $"{OrdersPrefix}.count";
    private const string OrderHistogramName = $"{OrdersPrefix}.duration";

    private readonly Counter<long> _orderRequestCounter;
    private readonly Histogram<double> _orderRequestDuration;

    public CreateOrderHeaderOpenTelemetry(IMeterFactory meterFactory)
        : base(meterFactory)
    {
        _orderRequestCounter = _meter.CreateCounter<long>(OrderCounterName);
        _orderRequestDuration = _meter.CreateHistogram<double>(OrderHistogramName, DurationUnit);
    }

    public void IncreaseOrderRequestCount(Ulid orderHeaderId)
    {
        _orderRequestCounter.Add(IncreaseCount, new KeyValuePair<string, object?>($"{OrdersPrefix}.id", orderHeaderId));
    }

    public RequestDuration MeasureRequestDuration()
    {
        return new RequestDuration(_orderRequestDuration);
    }
}

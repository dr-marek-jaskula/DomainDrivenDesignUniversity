using Shopway.Application.OpenTelemetry;
using System.Diagnostics.Metrics;

namespace Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;

public sealed class OrdersOpenTelemetry : OpenTelemetryBase
{
    private const string OrdersPrefix = $"{ShopwayApplicationPrefix}.orders";

    private const string OrderCounterName = $"{OrdersPrefix}.count";
    private const string OrderHistogramName = $"{OrdersPrefix}.duration";

    private readonly Counter<long> _orderRequestCounter;
    private readonly Histogram<double> _orderRequestDuration;

    public OrdersOpenTelemetry(IMeterFactory meterFactory) 
        : base(meterFactory)
    {
        _orderRequestCounter = _meter.CreateCounter<long>(OrderCounterName);
        _orderRequestDuration = _meter.CreateHistogram<double>(OrderHistogramName, DurationUnit);
    }

    public void IncreaseOrderRequestCount()
    {
        _orderRequestCounter.Add(1);
    }

    public RequestDuration MeasureRequestDuration()
    {
        return new RequestDuration(_orderRequestDuration);
    }
}
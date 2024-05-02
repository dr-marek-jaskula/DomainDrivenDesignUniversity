using System.Diagnostics.Metrics;

namespace Shopway.Application.OpenTelemetry;

public abstract class OpenTelemetryBase(IMeterFactory meterFactory)
{
    private const string MeterName = $"{nameof(Shopway)}.{nameof(Application)}";

    protected const string DurationUnit = "ms";
    protected Meter _meter = meterFactory.Create(MeterName);

    public class RequestDuration(Histogram<double> histogram) : IDisposable
    {
        private readonly long _requestStartTime = TimeProvider.System.GetTimestamp();
        private readonly Histogram<double> _histogram = histogram;

        public void Dispose()
        {
            var elapsed = TimeProvider.System.GetElapsedTime(_requestStartTime);
            _histogram.Record(elapsed.TotalMilliseconds);
            GC.SuppressFinalize(this);
        }
    }
}
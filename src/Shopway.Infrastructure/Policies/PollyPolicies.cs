using Polly;
using Polly.Retry;

namespace Shopway.Infrastructure.Policies;

public static class PollyPolicies
{
    private const int _maxRetries = 3;
    private static readonly Random _random = new();

    public static readonly AsyncRetryPolicy AsyncRetryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync
        (
            _maxRetries, 
            attempt => TimeSpan.FromMilliseconds(attempt * 50 + _random.Next(0, 20))
        );
}
using Polly;
using Polly.Retry;

namespace Shopway.Infrastructure.Policies;

public static class PollyPolicies
{
    private const int _maxRetries = 3;
    private const int _migrationMaxRetries = 5;
    private static readonly Random _random = new();

    public static readonly AsyncRetryPolicy AsyncRetryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync
        (
            _maxRetries, 
            attempt => TimeSpan.FromMilliseconds(attempt * 50 + _random.Next(0, 20))
        );

    public static readonly RetryPolicy MigrationRetryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetry
        (
            _migrationMaxRetries, 
            attempt => TimeSpan.FromMilliseconds(attempt * 50 + _random.Next(0, 20))
        );
}
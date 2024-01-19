using Polly;
using Polly.CircuitBreaker;
using Polly.RateLimit;
using Polly.Retry;
using Polly.Timeout;
using System.Collections.Frozen;
using System.Net.Sockets;

namespace Shopway.Infrastructure.Policies;

public static class PollyPipelines
{
    private const int _maxRetries = 3;
    private const int _initialDelay = 1;
    private const int _maxDelay = 16;
    private const int _migrationMaxRetries = 5;
    private static readonly Random _random = new();

    private static FrozenSet<Type> networkExceptions = new[]
    {
        typeof(SocketException),
        typeof(HttpRequestException),
    }.ToFrozenSet();

    private static FrozenSet<Type> strategyExceptions = new[]
    {
        typeof(TimeoutRejectedException),
        typeof(BrokenCircuitException),
        typeof(RateLimitRejectedException),
    }.ToFrozenSet();

    private static FrozenSet<Type> retryableExceptions = networkExceptions
        .Union(strategyExceptions)
        .ToFrozenSet();

    //Before Polly 8.0 approach
    public static readonly AsyncRetryPolicy AsyncRetryPolicy_Obsolete = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync
        (
            _maxRetries,
            attempt => TimeSpan.FromMilliseconds(attempt * 50 + _random.Next(0, 20))
        );

    public static readonly RetryPolicy MigrationRetryPolicy_Obsolete = Policy
        .Handle<Exception>()
        .WaitAndRetry
        (
            _migrationMaxRetries,
            attempt => TimeSpan.FromMilliseconds(attempt * 50 + _random.Next(0, 20))
        );

    //From Polly 8.0 approach
    public static readonly ResiliencePipeline AsyncRetryPipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = _maxRetries,
            BackoffType = DelayBackoffType.Exponential,
            Delay = TimeSpan.FromSeconds(_initialDelay),
            MaxDelay = TimeSpan.FromSeconds(_maxDelay),
            UseJitter = true,
            ShouldHandle = exception => new ValueTask<bool>(retryableExceptions.Contains(exception.GetType())),
            OnRetry = retryArguments =>
            {
                Console.WriteLine($"Current attempt: {retryArguments.AttemptNumber}, {retryArguments.Outcome.Exception}");
                return ValueTask.CompletedTask;
            }
        })
        .Build();

    public static readonly ResiliencePipeline MigrationRetryPipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            Name = nameof(MigrationRetryPipeline),
            MaxRetryAttempts = _migrationMaxRetries,
            DelayGenerator = args =>
            {
                var delay = TimeSpan.FromMicroseconds(args.AttemptNumber * 500 + _random.Next(0, 200));
                return new ValueTask<TimeSpan?>(delay);
            },
            ShouldHandle = args => args.Outcome switch
            {
                { Exception: not null } => PredicateResult.True(),
                _ => PredicateResult.False()
            }
        })
        .Build();
}
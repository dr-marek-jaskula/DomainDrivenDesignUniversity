using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shopway.Application.Middlewares;

/// <summary>
/// Examines if the request takes at least 'RequestDurationLogLevel' seconds. If so, then log a warning.
/// </summary>
public sealed class RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger) : IMiddleware
{
    private const int RequestDurationLogLevel = 4;
    private readonly ILogger<RequestTimeMiddleware> _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var startTime = Stopwatch.GetTimestamp();
        await next.Invoke(context);
        var requestDuration = Stopwatch.GetElapsedTime(startTime);

        if (requestDuration.Seconds >= RequestDurationLogLevel)
        {
            _logger.LogRequestTime(context.Request.Method, context.Request.Path, requestDuration.TotalMilliseconds);
        }
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 0,
        EventName = $"{nameof(RequestTimeMiddleware)}",
        Level = LogLevel.Warning,
        Message = "Request [{Method}] at {Path} took {Milliseconds} ms",
        SkipEnabledCheck = true
    )]
    public static partial void LogRequestTime(this ILogger logger, string method, PathString path, double milliseconds);
}

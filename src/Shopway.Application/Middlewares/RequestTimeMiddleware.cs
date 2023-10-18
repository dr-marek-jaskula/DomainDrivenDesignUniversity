using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Shopway.Application.Abstractions;

namespace Shopway.Application.Middlewares;

/// <summary>
/// Examines if the request takes at least 'RequestDurationLogLevel' seconds. If so, then log a warning.
/// </summary>
public sealed class RequestTimeMiddleware : IMiddleware
{
    private readonly ILoggerAdapter<RequestTimeMiddleware> _logger;
    private const int RequestDurationLogLevel = 4;

    public RequestTimeMiddleware(ILoggerAdapter<RequestTimeMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var startTime = Stopwatch.GetTimestamp();
        await next.Invoke(context);
        var requestDuration = Stopwatch.GetElapsedTime(startTime);

        if (requestDuration.Seconds >= RequestDurationLogLevel)
        {
            _logger.LogWarning("Request [{Method}] at {Path} took {Milliseconds} ms", context.Request.Method, context.Request.Path, requestDuration.Milliseconds);
        }
    }
}
using Microsoft.AspNetCore.Http;
using Shopway.Application.Abstractions;
using System.Diagnostics;
using static Microsoft.Extensions.Logging.LogLevel;

namespace Shopway.Application.Middlewares;

/// <summary>
/// Examines if the request takes at least 'RequestDurationLogLevel' seconds. If so, then log a warning
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
            var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {requestDuration.Milliseconds} ms";
            _logger.Log(Warning, message);
        }
    }
}
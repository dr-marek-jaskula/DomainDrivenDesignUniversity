using Microsoft.AspNetCore.Http;
using Shopway.Application.Abstractions;
using System.Diagnostics;
using static Microsoft.Extensions.Logging.LogLevel;

namespace Shopway.Application.Middlewares;

/// <summary>
/// Examines if the request takes at least 4 second. If yes, then log a warning
/// </summary>
public sealed class RequestTimeMiddleware : IMiddleware
{
    private readonly ILoggerAdapter<RequestTimeMiddleware> _logger;

    public RequestTimeMiddleware(ILoggerAdapter<RequestTimeMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var startTime = Stopwatch.GetTimestamp();
        await next.Invoke(context);
        var requestDuration = Stopwatch.GetElapsedTime(startTime);

        if (requestDuration.Seconds >= 4)
        {
            var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {requestDuration.Milliseconds} ms";
            _logger.Log(Warning, message);
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Shopway.Application.Middlewares;

/// <summary>
/// Examines if the request takes more then 4 second. If yes, then log the request as warning
/// </summary>
public sealed class RequestTimeMiddleware : IMiddleware
{
    private readonly ILogger<RequestTimeMiddleware> _logger;

    public RequestTimeMiddleware(ILogger<RequestTimeMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var startTime = Stopwatch.GetTimestamp();
        await next.Invoke(context);
        var requestDuration = Stopwatch.GetElapsedTime(startTime);

        if (requestDuration.Seconds > 4)
        {
            var message = $"Request [{context.Request.Method}] at {context.Request.Path} took {requestDuration.Milliseconds} ms";
            _logger.LogWarning(message);
        }
    }
}
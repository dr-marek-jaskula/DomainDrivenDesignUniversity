using MediatR;
using Microsoft.Extensions.Logging;
using Shopway.Domain.Abstractions;

namespace Shopway.Application.Pipelines;

public sealed class LoggingPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly ILogger<LoggingPipeline<TRequest, TResponse>> _logger;

    public LoggingPipeline(ILogger<LoggingPipeline<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle
    (
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation
        (
            "Starting request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow
        );

        var result = await next();

        if (result.IsSuccess)
        {
            _logger.LogInformation
            (
                "Request completed {@RequestName}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                DateTime.UtcNow
            );

            return result;
        }

        if (result is IValidationResult validationResult)
        {
            _logger.LogError
            (
                "Request failed {@RequestName}, {@ValidationErrors}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                validationResult.ValidationErrors,
                DateTime.UtcNow
            );

            return result;
        }

        _logger.LogError
        (
            "Request failed {@RequestName}, {@Error}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            result.Error,
            DateTime.UtcNow
        );

        return result;
    }
}
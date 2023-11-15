using MediatR;
using Shopway.Domain.Errors;
using Shopway.Domain.Abstractions;
using Microsoft.Extensions.Logging;

namespace Shopway.Application.Pipelines;

public sealed class LoggingPipeline<TRequest, TResponse>(ILogger<LoggingPipeline<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IResult
{
    private readonly ILogger<LoggingPipeline<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle
    (
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        _logger.LogStartingRequest(typeof(TRequest).Name, DateTime.UtcNow);
        
        var result = await next();

        if (result.IsSuccess)
        {
            _logger.LogCompletingRequest(typeof(TRequest).Name, DateTime.UtcNow);
            return result;
        }

        if (result is IValidationResult validationResult)
        {
            _logger.LogFailedRequestBasedOnValidationErrors(typeof(TRequest).Name, validationResult.ValidationErrors, DateTime.UtcNow);
            return result;
        }

        _logger.LogFailedRequestBasedOnSingleError(typeof(TRequest).Name, result.Error, DateTime.UtcNow);
        return result;
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 1,
        EventName = $"StartingRequest in {nameof(LoggingPipeline<IRequest<IResult>, IResult>)}",
        Level = LogLevel.Information,
        Message = "Starting request {RequestName}, {DateTimeUtc}",
        SkipEnabledCheck = false
    )]
    public static partial void LogStartingRequest(this ILogger logger, string requestName, DateTime dateTimeUtc);

    [LoggerMessage
    (
        EventId = 2,
        EventName = $"CompletingRequest in {nameof(LoggingPipeline<IRequest<IResult>, IResult>)}",
        Level = LogLevel.Information,
        Message = "Request completed {requestName}, {DateTimeUtc}",
        SkipEnabledCheck = false
    )]
    public static partial void LogCompletingRequest(this ILogger logger, string requestName, DateTime dateTimeUtc);

    [LoggerMessage
    (
        EventId = 3,
        EventName = $"FailedRequestBasedOnSingleError in {nameof(LoggingPipeline<IRequest<IResult>, IResult>)}",
        Level = LogLevel.Error,
        Message = "Request failed {RequestName}, {Error}, {DateTimeUtc}",
        SkipEnabledCheck = true
    )]
    public static partial void LogFailedRequestBasedOnSingleError(this ILogger logger, string requestName, Error error, DateTime dateTimeUtc);

    [LoggerMessage
    (
        EventId = 4,
        EventName = $"FailedRequestBasedOnValidationErrors in {nameof(LoggingPipeline<IRequest<IResult>, IResult>)}",
        Level = LogLevel.Error,
        Message = "Request failed {RequestName}, {ValidationErrors}, {DateTimeUtc}",
        SkipEnabledCheck = true
    )]
    public static partial void LogFailedRequestBasedOnValidationErrors(this ILogger logger, string requestName, Error[] validationErrors, DateTime dateTimeUtc);
}
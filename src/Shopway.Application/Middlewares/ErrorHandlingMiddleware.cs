using Shopway.Domain.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shopway.Application.Exceptions;
using static Shopway.Application.Utilities.ProblemDetailsUtilities;
using static Shopway.Application.Constants.Constants.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace Shopway.Application.Middlewares;

/// <summary>
/// Middleware that provides exception handling. Each request needs to be processed by the following try-catch block
/// </summary>
public sealed class ErrorHandlingMiddleware : IMiddleware
{
    private const string ProblemDetailsContentType = "application/problem+json";
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = exception switch
            {
                NotFoundException => 404,
                BadRequestException => 400,
                ForbidException => 403,
                _ => 500
            };

            await HandleExceptionAsync(context, exception);
        }
    }

    /// <summary>
    /// Handler that sends enough information to user, so that he would be able to identify the problem and explain the details to developer.
    /// However, the detail does not cover the whole problem, in order to prevent the user from getting the sensitive data or unreadable for user data.
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="exception">Request exception</param>
    /// <returns>Exception details in JSON format</returns>
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = CreateProblemDetails
        (
           title: ExceptionOccurred,
           status: context.Response.StatusCode,
           error: Error.Exception(exception.Message),
           context: context
        );

        _logger
            .LogUnexpectedException(context.Request.Method, context.Request.Path, exception.GetType().Name, exception.Source, exception.Message, exception.StackTrace);

        await context
            .Response
            .WriteAsJsonAsync(problemDetails, typeof(ProblemDetails), options: null, contentType: ProblemDetailsContentType);
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 10,
        EventName = $"{nameof(ErrorHandlingMiddleware)}",
        Level = LogLevel.Error,
        Message = "Request [{Method}] at {Path} thrown an exception '{Name}' from source '{Source}'. \n Exception message: '{Message}'. \n StackTrace: '{StackTrace}'",
        SkipEnabledCheck = true
    )]
    public static partial void LogUnexpectedException(this ILogger logger, string method, PathString path, string name, string? source, string message, string? stackTrace);
}
using Microsoft.AspNetCore.Mvc;
using Shopway.Presentation.Exceptions;

namespace Shopway.App.Middlewares;

/// <summary>
/// Middleware that provides exception handling. Each request needs to be processed by the following try-catch block
/// </summary>
public class ErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (NotFoundException notFoundException)
        {
            context.Response.StatusCode = 404;
            await HandleExceptionAsync(context, notFoundException);
        }
        catch (BadRequestException badRequestException)
        {
            context.Response.StatusCode = 400;
            await HandleExceptionAsync(context, badRequestException);
        }
        catch (ForbidException forbidException)
        {
            context.Response.StatusCode = 403;
            await HandleExceptionAsync(context, forbidException);
        }
        catch (Exception exception)
        {
            context.Response.StatusCode = 500;
            await HandleExceptionAsync(context, exception);
        }
    }

    /// <summary>
    /// Handler that sends the enough information to user so that he would be able to identify the problem and explain the details to developer.
    /// However, the detail does not cover the whole problem, in order to prevent the user from getting the sensitive data or unreadable for user data.
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="exception">Request exception</param>
    /// <returns>Exception details in JSON format</returns>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var problemDetails = new ProblemDetails //this class represents the standard details. Very important class in real-life web api
        {
            Type = "https://Shopway.com",
            Title = "An unrecoverable error occurred",
            Status = context.Response.StatusCode,
            Detail = "Problem details: " + exception.Message,
            Instance = context.Request.Path
        };
        
        problemDetails.Extensions.Add("RequestId", context.TraceIdentifier);
        await context.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType(), options: null, contentType: "application/problem+json");
    }
}

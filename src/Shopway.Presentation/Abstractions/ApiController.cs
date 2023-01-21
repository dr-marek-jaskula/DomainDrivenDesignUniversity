using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Errors;
using IResult = Shopway.Domain.Abstractions.IResult;

namespace Shopway.Presentation.Abstractions;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected IActionResult HandleFailure(IResult result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException("Result was successful"),

            IValidationResult validationResult => BadRequest(
                    CreateProblemDetails(
                        "Validation Error", 
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),

            _ => BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };
    }

    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null)
    {
        return new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
    }

    protected IActionResult QueryResult<T>(IResult<T> response)
    {
        return response.IsSuccess 
            ? Ok(response.Value) 
            : NotFound(response.Error);
    }

    protected IActionResult CreatedAtActionResult<T>(IResult<T> response, string? actionName)
    {
        return CreatedAtAction(
            actionName,
            new { id = response.Value },
            response.Value);
    }
}

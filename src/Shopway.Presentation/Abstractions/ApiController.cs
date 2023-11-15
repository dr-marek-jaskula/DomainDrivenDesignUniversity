using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Shopway.Domain.Abstractions;
using IResult = Shopway.Domain.Abstractions.IResult;
using static Shopway.Application.Utilities.ProblemDetailsUtilities;
using static Shopway.Application.Constants.Constants.ProblemDetails;

namespace Shopway.Presentation.Abstractions;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ApiController(ISender sender) : ControllerBase
{
    protected readonly ISender Sender = sender;

    protected IActionResult HandleFailure(IResult result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException("Result was successful"),

            IValidationResult validationResult => BadRequest
            (
                CreateProblemDetails
                (
                    ValidationError, 
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    validationResult.ValidationErrors
                )
            ),

            _ => BadRequest
            (
                CreateProblemDetails
                (
                    InvalidRequest,
                    StatusCodes.Status400BadRequest,
                    result.Error
                )
            )
        };
    }

    protected IActionResult CreatedAtActionResult<T>(IResult<T> result, string? actionName)
    {
        return CreatedAtAction
        (
            actionName,
            new { id = result.Value },
            result.Value
        );
    }
}

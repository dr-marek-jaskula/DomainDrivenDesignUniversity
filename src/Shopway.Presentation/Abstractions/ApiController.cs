using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopway.Domain.Abstractions;
using IResult = Shopway.Domain.Abstractions.IResult;
using static Shopway.Application.Utilities.ProblemDetailsUtilities;
using static Shopway.Application.Constants.ProblemDetailsConstants;

namespace Shopway.Presentation.Abstractions;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
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
                        ValidationError, 
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.ValidationErrors)),

            _ => BadRequest(
                    CreateProblemDetails(
                        InvalidRequest,
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };
    }

    protected IActionResult CreatedAtActionResult<T>(IResult<T> response, string? actionName)
    {
        return CreatedAtAction(
            actionName,
            new { id = response.Value },
            response.Value);
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Filters;

namespace Shopway.Presentation.OpenApi.Examples;

public sealed class ProblemDetailsExample : IExamplesProvider<ProblemDetails>
{
    public ProblemDetails GetExamples()
    {
        return new ProblemDetails
        {
            Type = "Type of the error",
            Title = "Title of the error",
            Status = StatusCodes.Status400BadRequest,
            Detail = "Error message",
            Extensions = { { "errors", "List of validation errors" } }
        };
    }
}
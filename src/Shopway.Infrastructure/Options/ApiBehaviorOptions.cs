using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames.Application;
using static Shopway.Application.Utilities.ProblemDetailsUtilities;
using static Shopway.Application.Constants.ProblemDetailsConstants;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState;

namespace Shopway.Infrastructure.Options;

public static class ApiBehaviorOptions
{
    public static Func<ActionContext, IActionResult> InvalidModelStateResponse =>
        context =>
        {
            var errors = context.ModelState.Values
                .Where(modelStateEntry => modelStateEntry.ValidationState is Invalid)
                .SelectMany(modelStateEntry => modelStateEntry
                    .Errors
                    .Select(error => error.ErrorMessage))
                .ToList();

            var problemDetails = CreateProblemDetails
            (
                InvalidRequest,
                InvalidRequestTitle,
                StatusCodes.Status400BadRequest,
                errors
            );

            var result = new BadRequestObjectResult(problemDetails);

            result.ContentTypes.Add(Json);

            return result;
        };
}

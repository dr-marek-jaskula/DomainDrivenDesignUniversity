using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState;
using static Shopway.Application.Constants.Constants.ProblemDetails;
using static Shopway.Application.Utilities.ProblemDetailsUtilities;
using static System.Net.Mime.MediaTypeNames.Application;

namespace Shopway.Presentation.Options;

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

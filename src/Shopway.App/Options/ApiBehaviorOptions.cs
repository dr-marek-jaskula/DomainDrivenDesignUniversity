using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames.Application;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState;
using static Shopway.Presentation.Utilities.ProblemDetailsUtilities;

namespace Shopway.App.Options;

public static class ApiBehaviorOptions
{
    public static Func<ActionContext, IActionResult> InvalidModelStateResponse =>
        context =>
        {
            var errors = context.ModelState.Values
                .Where(modelStateEntry => modelStateEntry.ValidationState is Invalid)
                .SelectMany(modelStateEntry => modelStateEntry.Errors
                    .Select(error => error.ErrorMessage))
                .ToList();

            var problemDetails = CreateProblemDetails(
                        "https://Shopway.com",
                        "Invalid request body or request parameters",
                        StatusCodes.Status400BadRequest,
                        errors);

            var result = new BadRequestObjectResult(problemDetails);

            result.ContentTypes.Add(Json);

            return result;
        };
}

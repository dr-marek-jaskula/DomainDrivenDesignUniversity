using System.Net;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames.Application;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState;

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

            var result = new BadRequestObjectResult(new
            {
                Status = (int)HttpStatusCode.BadRequest,
                Code = "Invalid request body or request parameters",
                Errors = errors
            });

            result.ContentTypes.Add(Json);

            return result;
        };
}

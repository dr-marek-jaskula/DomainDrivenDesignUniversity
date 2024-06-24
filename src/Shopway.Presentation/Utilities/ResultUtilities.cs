using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Results.Abstractions;
using static Shopway.Application.Constants.Constants.ProblemDetails;
using static Shopway.Application.Utilities.ProblemDetailsUtilities;
using IResult = Shopway.Domain.Common.Results.IResult;

namespace Shopway.Presentation.Utilities;

public static class ResultUtilities
{
    public static Ok<TValue> ToOkResult<TValue>(this IResult<TValue> result)
    {
        return result switch
        {
            { IsSuccess: true } => TypedResults.Ok(result.Value),
            _ => throw new InvalidOperationException("Result was failed")
        };
    }

    public static BadRequest<TValue> ToBadRequestResult<TValue>(this IResult<TValue> result)
    {
        return result switch
        {
            { IsSuccess: true } => TypedResults.BadRequest(result.Value),
            _ => throw new InvalidOperationException("Result was failed")
        };
    }

    public static Ok ToOkResult(this IResult result)
    {
        return result switch
        {
            { IsSuccess: true } => TypedResults.Ok(),
            _ => throw new InvalidOperationException("Result was failed")
        };
    }

    public static ForbidHttpResult ToForbidResult(this AuthorizationResult result)
    {
        return result switch
        {
            { Succeeded: true } => throw new InvalidOperationException("Result was successful"),
            _ => TypedResults.Forbid()
        };
    }

    public static ProblemHttpResult ToProblemHttpResult(this IResult result)
    {
        return result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException("Result was successful"),

            IValidationResult validationResult => TypedResults.Problem
            (
                CreateProblemDetails
                (
                    ValidationError,
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    validationResult.ValidationErrors
                )
            ),

            _ => TypedResults.Problem
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
}

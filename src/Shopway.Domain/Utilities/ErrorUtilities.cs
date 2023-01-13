using Shopway.Domain.Abstractions;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;

namespace Shopway.Domain.Utilities;

public static class ErrorUtilities
{
    public static TResult CreateValidationResult<TResult>(this ICollection<Error> errors)
        where TResult : class, IResult
    {
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        object validationResult = typeof(ValidationResult<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, new object?[] { errors })!;

        return (TResult)validationResult;
    }
}
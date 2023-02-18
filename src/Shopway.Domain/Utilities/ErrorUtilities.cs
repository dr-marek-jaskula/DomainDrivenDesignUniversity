using Shopway.Domain.Abstractions;
using Shopway.Domain.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.ValueObjects;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq.Expressions;
using System.Reflection.Emit;

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

    /// <summary>
    /// Creates the ValidationResult. Instead of value object instance, the delegate, that defines the way to create value object, is required. 
    /// The reason is that we do not want to allow creating the value object if there is at least one error
    /// </summary>
    /// <typeparam name="TValueObject"></typeparam>
    /// <param name="errors">Not null collection of errors</param>
    /// <param name="createValueObject">Delegate that specifies how to create the value object</param>
    /// <returns>ValidationReuslt</returns>
    public static ValidationResult<TValueObject> CreateValidationResult<TValueObject>
    (
        this ICollection<Error> errors,
        Func<TValueObject> createValueObject
    )
        where TValueObject : ValueObject
    {
        if (errors.Any())
        {
            return ValidationResult<TValueObject>.WithErrors(errors.ToArray());
        }

        return ValidationResult<TValueObject>.WithoutErrors(createValueObject());
    }
}
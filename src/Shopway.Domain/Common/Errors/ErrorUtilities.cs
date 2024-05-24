using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.Results;

namespace Shopway.Domain.Common.Errors;

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
            .Invoke(null, [errors])!;

        return (TResult)validationResult;
    }

    /// <summary>
    /// Creates the ValidationResult. Instead of value object instance, the delegate, that defines the way to create value object, is required. 
    /// The reason is that we do not want to allow creating the value object if there is at least one error
    /// </summary>
    /// <typeparam name="TValueObject"></typeparam>
    /// <param name="errors">Not null collection of errors</param>
    /// <param name="createValueObject">Delegate that specifies how to create the value object</param>
    /// <returns>ValidationResult</returns>
    /// <exception cref="ArgumentNullException">Thrown if errors collection is null</exception>
    public static ValidationResult<TValueObject> CreateValidationResult<TValueObject>
    (
        this ICollection<Error> errors,
        Func<TValueObject> createValueObject
    )
        where TValueObject : ValueObject
    {
        if (errors is null)
        {
            throw new ArgumentNullException($"{nameof(errors)} must not be null");
        }

        if (errors.Count != 0)
        {
            return ValidationResult<TValueObject>.WithErrors(errors.ToArray());
        }

        return ValidationResult<TValueObject>.WithoutErrors(createValueObject.Invoke());
    }

    /// <summary>
    /// Add error to the list if the condition is true
    /// </summary>
    /// <param name="errors">Error list</param>
    /// <param name="condition">Validation condition</param>
    /// <param name="error">Error to add to list if the condition is true</param>
    /// <returns>Same instance of error list</returns>
    public static IList<Error> If
    (
        this IList<Error> errors,
        bool condition,
        params Error[] errorsToAdd
    )
    {
        if (condition is true)
        {
            foreach (Error errorToAdd in errorsToAdd)
            {
                errors.Add(errorToAdd);
            }
        }

        return errors;
    }

    /// <summary>
    /// Use external validation that usually group multiple validations into logic group
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errors">Error list</param>
    /// <param name="validationSegment">Func that group validations into logic group</param>
    /// <param name="valueUnderValidation">Value that is being validated</param>
    /// <returns>Same instance of error list</returns>
    public static IList<Error> UseValidation<TValue>
    (
        this IList<Error> errors,
        Func<IList<Error>, TValue, IList<Error>> validationSegment,
        TValue valueUnderValidation
    )
    {
        return validationSegment(errors, valueUnderValidation);
    }

    /// <summary>
    /// Use external validation that usually group multiple validations into logic group
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="errors">Error list</param>
    /// <param name="validationSegment">Func that group validations into logic group</param>
    /// <param name="valueUnderValidation">Value that is being validated</param>
    /// <returns>Same instance of error list</returns>
    public static IList<Error> UseValidation<TValue>
    (
        this IList<Error> errors,
        Func<TValue, IList<Error>> validationSegment,
        TValue valueUnderValidation
    )
    {
        var errorsToAdd = validationSegment(valueUnderValidation);

        foreach (var errorToAdd in errorsToAdd)
        {
            errorsToAdd.Add(errorToAdd);
        }

        return errors;
    }
}

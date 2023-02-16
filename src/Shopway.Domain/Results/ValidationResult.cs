using Shopway.Domain.Abstractions;
using Shopway.Domain.Errors;

namespace Shopway.Domain.Results;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] validationErrors)
        : base(default, false, IValidationResult.ValidationError)
    {
        ValidationErrors = validationErrors;
    }

    private ValidationResult(TValue? value)
        : base(value, true, Error.None)
    {
        ValidationErrors = Array.Empty<Error>();
    }

    public Error[] ValidationErrors { get; }

    /// <summary>
    /// Creates failure ValidationResult<typeparamref name="TValue"/>
    /// </summary>
    /// <param name="validationErrors"></param>
    /// <returns></returns>
    public static ValidationResult<TValue> WithErrors(Error[] validationErrors)
    {
        return new(validationErrors);
    }

    /// <summary>
    /// Creates success ValidationResult<typeparamref name="TValue"/>
    /// </summary>
    /// <param name="value">Result value</param>
    /// <returns>Success ValidationResult</returns>
    public static ValidationResult<TValue> WithoutErrors(TValue? value)
    {
        return new(value);
    }
}

public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] validationErrors)
        : base(false, IValidationResult.ValidationError)
    {
        ValidationErrors = validationErrors;
    }

    private ValidationResult()
        : base(true, Error.None)
    {
        ValidationErrors = Array.Empty<Error>();
    }

    public Error[] ValidationErrors { get; }

    /// <summary>
    /// Creates failure ValidationResult
    /// </summary>
    /// <param name="validationErrors">Validation errors</param>
    /// <returns>Failure ValidationResult</returns>
    public static ValidationResult WithErrors(ICollection<Error> validationErrors)
    {
        return new(validationErrors.ToArray());
    }

    /// <summary>
    /// Creates success ValidationResult 
    /// </summary>
    /// <returns>Success ValidationResult</returns>
    public static ValidationResult WithoutErrors()
    {
        return new();
    }
}

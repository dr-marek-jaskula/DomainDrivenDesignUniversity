using Shopway.Domain.Common.Results.Abstractions;
using Shopway.Domain.Errors;

namespace Shopway.Domain.Common.Results;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] validationErrors)
        : base(default, Error.ValidationError)
    {
        ValidationErrors = validationErrors;
    }

    private ValidationResult(TValue? value)
        : base(value, Error.None)
    {
        ValidationErrors = Array.Empty<Error>();
    }

    public Error[] ValidationErrors { get; }

    /// <summary>
    /// Creates failure ValidationResult<typeparamref name="TValue"/>
    /// </summary>
    /// <param name="validationErrors">Validation errors</param>
    /// <returns>Failure ValidationResult</returns>
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
    private static readonly ValidationResult _successValidationResult = new();

    private ValidationResult(Error[] validationErrors)
        : base(Error.ValidationError)
    {
        ValidationErrors = validationErrors;
    }

    private ValidationResult()
        : base(Error.None)
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
        return _successValidationResult;
    }
}

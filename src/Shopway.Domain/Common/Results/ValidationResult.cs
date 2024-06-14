using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results.Abstractions;

namespace Shopway.Domain.Common.Results;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private const int MaximalErrorsLength = 40;

    private ValidationResult(Error[] validationErrors)
        : base(default, Error.ValidationError)
    {
        ValidationErrors = ValidationResult<TValue>.KeepErrorsLenghtNotTooLongForSecurityReasons(validationErrors);
    }

    private ValidationResult(TValue? value)
        : base(value, Error.None)
    {
        ValidationErrors = [];
    }

    public Error[] ValidationErrors { get; }

    /// <summary>
    /// Creates failure ValidationResult<typeparamref name="TValue"/>
    /// </summary>
    /// <param name="validationErrors">Validation errors</param>
    /// <returns>Failure ValidationResult</returns>
    public static ValidationResult<TValue> WithErrors(params Error[] validationErrors)
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

    /// <summary>
    /// To prevent malicious users to generate a very high number of errors to be send (in order to overload the network), we limit the maximal number of errors
    /// </summary>
    private static Error[] KeepErrorsLenghtNotTooLongForSecurityReasons(Error[] validationErrors)
    {
        return validationErrors.Length > MaximalErrorsLength
            ? validationErrors.Take(MaximalErrorsLength).ToArray()
            : validationErrors;
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
        ValidationErrors = [];
    }

    public Error[] ValidationErrors { get; }

    /// <summary>
    /// Creates failure ValidationResult
    /// </summary>
    /// <param name="validationErrors">Validation errors</param>
    /// <returns>Failure ValidationResult</returns>
    public static ValidationResult WithErrors(ICollection<Error> validationErrors)
    {
        return new([.. validationErrors]);
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

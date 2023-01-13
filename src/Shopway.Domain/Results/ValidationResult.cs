using Shopway.Domain.Abstractions;
using Shopway.Domain.Errors;

namespace Shopway.Domain.Results;

public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(default, false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    //This is the way to create the ValidationResult<TValue>
    public static ValidationResult<TValue> WithErrors(Error[] errors)
    {
        return new(errors);
    }
}

public sealed class ValidationResult : Result, IValidationResult
{
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError)
    {
        Errors = errors;
    }

    public Error[] Errors { get; }

    //This is the way to create the ValidationResult
    public static ValidationResult WithErrors(ICollection<Error> errors)
    {
        return new(errors.ToArray());
    }
}

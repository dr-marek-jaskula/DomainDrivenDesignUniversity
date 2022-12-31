using Shopway.Application.Abstractions;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;

namespace Shopway.Infrastructure.Validators;

public sealed class Validator : IValidator
{
    private readonly List<Error> _errors;

    public Validator()
    {
        _errors = new();
        IsValid = true;
    }

    public bool IsValid { get; private set; }
    public bool IsInvalid => IsValid is false;

    public IValidator If(bool invalid, Error thenError)
    {
        if (invalid is false)
        {
            return this;
        }

        _errors.Add(thenError);
        IsValid = false;
        return this;
    }

    public IValidator If(Func<bool> validate, Error thenError)
    {
        return If(validate(), thenError);
    }

    public IValidator Validate(Result<object> valueObject)
    {
        if (valueObject.IsSuccess)
        {
            return this;
        }

        _errors.Add(valueObject.Error);
        IsValid = false;
        return this;
    }

    public Error Error => _errors.CreateValidationResult<ValidationResult>().Error;
}
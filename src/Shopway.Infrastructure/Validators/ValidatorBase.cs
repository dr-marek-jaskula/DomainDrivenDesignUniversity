using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;
using Shopway.Domain.Utilities;

namespace Shopway.Infrastructure.Validators;

public abstract class ValidatorBase<TValidator>
    where TValidator : ValidatorBase<TValidator>
{
    private readonly List<Error> _errors;

    protected ValidatorBase()
    {
        _errors = new();
        IsValid = true;
    }

    public bool IsValid;
    public bool IsInvalid => IsValid is false;

    public TValidator If(bool invalid, Error thenError)
    {
        if (invalid is false)
        {
            return (TValidator)this;
        }

        _errors.Add(thenError);
        IsValid = false;
        return (TValidator)this;
    }

    public TValidator If(Func<bool> validate, Error thenError)
    {
        return If(validate(), thenError);
    }

    public TValidator If(Result<ValueObject> valueObject)
    {
        if (valueObject.IsSuccess)
        {
            return (TValidator)this;
        }

        _errors.Add(valueObject.Error);
        IsValid = false;
        return (TValidator)this;
    }

    protected Result CreateResult() => _errors.CreateValidationResult<ValidationResult>();
}
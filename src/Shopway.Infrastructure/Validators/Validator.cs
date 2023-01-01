using Microsoft.AspNetCore.Authentication.OAuth;
using Shopway.Application.Abstractions;
using Shopway.Application.CQRS.Products.Commands.CreateProduct;
using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
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

    public IValidator Validate<TValueObject>(Result<TValueObject> valueObject)
        where TValueObject : ValueObject
    {
        if (valueObject.IsSuccess)
        {
            return this;
        }

        _errors.Add(valueObject.Error);
        IsValid = false;
        return this;
    }

    public ValidationResult<TResponse> Failure<TResponse>()
        where TResponse : IResponse
    {
        return ValidationResult<TResponse>.WithErrors(_errors.ToArray());
    }
}
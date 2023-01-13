using Shopway.Domain.Abstractions.BaseTypes;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions;

public interface IValidator
{
    bool IsValid { get; }
    bool IsInvalid { get; }

    IValidator If(bool invalid, Error thenError);
    IValidator Validate<TValueObject>(Result<TValueObject> valueObject) 
        where TValueObject : ValueObject;
    ValidationResult<TResponse> Failure<TResponse>()
            where TResponse : IResponse;
}
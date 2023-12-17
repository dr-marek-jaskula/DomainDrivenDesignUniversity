using Shopway.Domain.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.BaseTypes;

namespace Shopway.Application.Abstractions;

public interface IValidator
{
    bool IsValid { get; }
    bool IsInvalid { get; }

    IValidator If(bool condition, Error thenError);
    IValidator Validate<TValueObject>(Result<TValueObject> result) 
        where TValueObject : ValueObject;
    IValidator Validate<TValueObject>(ValidationResult<TValueObject> validationResult) 
        where TValueObject : ValueObject;
    ValidationResult<TResponse> Failure<TResponse>()
            where TResponse : IResponse;
    ValidationResult Failure();
}
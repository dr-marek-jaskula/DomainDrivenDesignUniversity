using Shopway.Domain.Errors;
using Shopway.Domain.Primitives;
using Shopway.Domain.Results;

namespace Shopway.Application.Abstractions;

public interface IValidator
{
    bool IsValid { get; }
    bool IsInvalid { get; }

    public IValidator If(bool invalid, Error thenError);
    IValidator If(Func<bool> validate, Error thenError);
    public IValidator Validate(Result<object> valueObject);
    Error Error { get; }
}
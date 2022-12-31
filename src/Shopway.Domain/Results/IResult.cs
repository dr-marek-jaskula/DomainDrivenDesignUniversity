using Shopway.Domain.Errors;

namespace Shopway.Domain.Results;

public interface IResult<out TValue> : IResult
{
    TValue Value { get; }
}

public interface IResult
{
    bool IsSuccess { get; }

    bool IsFailure { get; }

    Error Error { get; }
}
using Shopway.Domain.Common.Errors;

namespace Shopway.Domain.Common.Results;

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
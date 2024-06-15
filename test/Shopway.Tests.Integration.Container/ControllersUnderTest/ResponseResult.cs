using Shopway.Domain.Common.Errors;

namespace Shopway.Tests.Integration.ControllersUnderTest;

/// <summary>
/// Represents a helper class used to deserialize the response
/// </summary>
public sealed class ResponseResult<TValue> : ResponseResult
{
    public TValue? Value { get; set; }

    public ResponseResult(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }
}

/// <summary>
/// Represents a helper class used to deserialize the response
/// </summary>
public class ResponseResult
{
    public ResponseResult(bool isSuccess, Error error)
    {
        bool successWithError = isSuccess && error != Error.None;

        if (successWithError)
        {
            throw new InvalidOperationException("Result was successful but error was not Error.None");
        }

        bool failureWithNoError = !isSuccess && error == Error.None;

        if (failureWithNoError)
        {
            throw new InvalidOperationException("Result was failure but error was Error.None");
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; set; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }
}

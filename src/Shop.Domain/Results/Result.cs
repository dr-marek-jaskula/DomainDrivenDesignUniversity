using Shopway.Domain.Errors;

namespace Shopway.Domain.Results;

public class Result<TValue> : Result
{
    private readonly TValue? _value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("The value of a failure result can not be accessed.");

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public static implicit operator Result<TValue>(TValue? value)
    {
        return Create(value);
    }
}

public class Result
{
    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    protected internal Result(bool isSuccess, Error error)
    {
        bool successWithError = isSuccess && error != Error.None;

        if (successWithError)
        {
            throw new InvalidOperationException();
        }

        bool failureWithNoError = !isSuccess && error == Error.None;

        if (failureWithNoError)
        {
            throw new InvalidOperationException();
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result<TValue> Create<TValue>(TValue? value)
    {
        return value is not null
            ? Success(value)
            : Failure<TValue>(Error.NullValue);
    }

    public static Result Success()
    {
        return new(true, Error.None);
    }

    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new(value, true, Error.None);
    }

    public static Result Failure(Error error)
    {
        return new(false, error);
    }

    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new(default, false, error);
    }
}
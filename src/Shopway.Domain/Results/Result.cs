using Shopway.Domain.Abstractions;
using Shopway.Domain.Errors;

namespace Shopway.Domain.Results;

/// <summary>
/// Represents the result, with status, possible value and possible error
/// </summary>
/// <typeparam name="TValue">The result value type.</typeparam>
public class Result<TValue> : Result, IResult<TValue>
{
    private readonly TValue? _value;

    /// <summary>
    /// Gets the result value if the result is successful, otherwise throws an exception
    /// </summary>
    /// <returns>The result value if the result is successful</returns>
    /// <exception cref="InvalidOperationException"> when <see cref="Result.IsFailure"/> is true</exception>
    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException($"The value of a failure result can not be accessed. Type '{typeof(TValue).FullName}'.");

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{TValueType}"/> class with the specified parameters
    /// </summary>
    /// <param name="value">The result value</param>
    /// <param name="isSuccess">The flag indicating if the result is successful</param>
    /// <param name="error">The error</param>
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

/// <summary>
/// Represents a result, with a status and possible error
/// </summary>
public class Result : IResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class with the specified parameters
    /// </summary>
    /// <param name="isSuccess">The flag indicating if the result is successful</param>
    /// <param name="error">The error that occurred</param>
    private protected Result(bool isSuccess, Error error)
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

    /// <summary>
    /// Gets a value indicating whether the result is a success
    /// </summary>
    public bool IsSuccess { get; }

    /// <summary>
    /// Gets a value indicating whether the result is a failure
    /// </summary>
    public bool IsFailure => !IsSuccess;

    /// <summary>
    /// Gets the error
    /// </summary>
    public Error Error { get; }

    /// <summary>
    /// Returns a success <see cref="Result"/>
    /// </summary>
    /// <param name="condition">The condition</param>
    /// <returns>A new instance of <see cref="Result"/></returns>
    public static Result Create(bool condition)
    {
        return condition 
            ? Success() 
            : Failure(Error.ConditionNotSatisfied);
    }

    /// <summary>
    /// Returns a success <see cref="Result"/>
    /// </summary>
    /// <param name="condition">The condition</param>
    /// <returns>A new instance of <see cref="Result"/></returns>
    public static Result<TValue> Create<TValue>(TValue? value)
    {
        return value is not null
            ? Success(value)
            : Failure<TValue>(Error.NullValue);
    }

    /// <summary>
    /// Returns a success <see cref="Result"/>
    /// </summary>
    /// <returns>A new instance of <see cref="Result"/></returns>
    public static Result Success()
    {
        return new(true, Error.None);
    }

    /// <summary>
    /// Returns a success <see cref="Result{TValue}"/> with the specified value
    /// </summary>
    /// <typeparam name="TValue">The result type.</typeparam>
    /// <param name="value">The result value.</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> with the specified value</returns>
    public static Result<TValue> Success<TValue>(TValue value)
    {
        return new(value, true, Error.None);
    }

    /// <summary>
    /// Returns a failure <see cref="Result"/> with the specified error
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>A new instance of <see cref="Result"/> with the specified error</returns>
    public static Result Failure(Error error)
    {
        return new(false, error);
    }

    /// <summary>
    /// Returns a failure <see cref="Result{TValue}"/> with the specified error
    /// </summary>
    /// <typeparam name="TValue">The result type</typeparam>
    /// <param name="error">The error</param>
    /// <returns>A new instance of <see cref="Result{TValue}"/> with the specified error and failure flag set</returns>
    public static Result<TValue> Failure<TValue>(Error error)
    {
        return new(default, false, error);
    }
}
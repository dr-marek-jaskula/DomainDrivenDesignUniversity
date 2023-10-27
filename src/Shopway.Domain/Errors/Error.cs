namespace Shopway.Domain.Errors;

/// <summary>
/// Represents an error that contains the informations about the failure
/// </summary>
public class Error : IEquatable<Error>
{
    /// <summary>
    /// The empty error instance used to represent that no error has occurred
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// The null value error instance
    /// </summary>
    public static readonly Error NullValue = new($"{nameof(Error)}.{nameof(NullValue)}", "The result value is null");

    /// <summary>
    /// The condition not satisfied error instance
    /// </summary>
    public static readonly Error ConditionNotSatisfied = new($"{nameof(Error)}.{nameof(ConditionNotSatisfied)}", "The specified condition was not satisfied.");

    /// <summary>
    /// The validation error instance
    /// </summary>
    public static readonly Error ValidationError = new($"{nameof(ValidationError)}", "A validation problem occurred.");

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class
    /// </summary>
    /// <param name="code">The error code</param>
    /// <param name="message">The error message</param>
    public static Error New(string code, string message)
    {
        return new Error(code, message);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class from provided exception
    /// </summary>
    /// <param name="exception">The thrown exception</param>
    public static Error FromException<TException>(TException exception)
        where TException : Exception
    {
        return New(exception.GetType().Name, exception.Message);
    }

    /// <summary>
    /// Gets the error code
    /// </summary>
    public string Code { get; }

    /// <summary>
    /// Gets the error message
    /// </summary>
    public string Message { get; }

    public static implicit operator string(Error error)
    {
        return error.Code;
    }

    public static bool operator ==(Error? a, Error? b)
    {
        if (a is null && b is null)
        {
            return true;
        }

        if (a is null || b is null)
        {
            return false;
        }

        return a.Equals(b);
    }

    public static bool operator !=(Error? a, Error? b)
    {
        return !(a == b);
    }

    /// <inheritdoc />
    public virtual bool Equals(Error? other)
    {
        if (other is null)
        {
            return false;
        }

        return Code == other.Code 
            && Message == other.Message;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Error error && Equals(error);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Code, Message);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Message;
    }

    public void ThrowIfErrorNone()
    {
        if (this == None)
        {
            throw new InvalidOperationException("Provided error is Error.None");
        }
    }

    public string? MessageOrNullIfErrorNone()
    {
        if (this == None)
        {
            return null;
        }

        return Message;
    }
}

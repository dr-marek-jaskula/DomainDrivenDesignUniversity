namespace Shopway.Domain.Common.Errors;

/// <summary>
/// Represents an error that contains the informations about the failure
/// </summary>
public sealed partial class Error : IEquatable<Error>
{
    private const string SerializationSeparator = ": ";
    private const int MaximalErrorMessageLenght = 500;

    /// <summary>
    /// The empty error instance used to represent that no error has occurred
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

    /// <summary>
    /// The null value error instance
    /// </summary>
    public static readonly Error NullValue = new($"{nameof(NullValue)}", "The result value is null.");

    /// <summary>
    /// The condition not satisfied error instance
    /// </summary>
    public static readonly Error ConditionNotSatisfied = new($"{nameof(ConditionNotSatisfied)}", "The specified condition was not satisfied.");

    /// <summary>
    /// The validation error instance
    /// </summary>
    public static readonly Error ValidationError = new($"{nameof(ValidationError)}", "A validation problem occurred.");

    public Error(string code, string message)
    {
        Code = code;
        Message = KeepMessageNotTooLongForSecurityReasons(message);
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
        if (exception is AggregateException || exception.InnerException is null)
        {
            return New(exception.GetType().Name, exception.Message);
        }

        return New(exception.GetType().Name, $"{exception.Message}. ({exception.InnerException.Message})");
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

    public static bool operator ==(Error? first, Error? second)
    {
        if (first is null && second is null)
        {
            return true;
        }

        if (first is null || second is null)
        {
            return false;
        }

        return first.Equals(second);
    }

    public static bool operator !=(Error? first, Error? second)
    {
        return !(first == second);
    }

    /// <inheritdoc />
    public bool Equals(Error? other)
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

    public string Serialize()
    {
        return $"{Code}{SerializationSeparator}{Message}";
    }

    public static Error Deserialize(string serializedError)
    {
        var splitted = serializedError.Split(SerializationSeparator);
        return New(splitted[0], splitted[1]);
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

    /// <summary>
    /// To prevent malicious users to generate a very long error to be send (in order to overload the network), we limit the maximal number of errors
    /// </summary>
    public static string KeepMessageNotTooLongForSecurityReasons(string message)
    {
        return message.Length > MaximalErrorMessageLenght
            ? message[..MaximalErrorMessageLenght]
            : message;
    }
}

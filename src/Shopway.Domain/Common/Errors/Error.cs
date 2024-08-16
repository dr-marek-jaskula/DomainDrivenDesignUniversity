// Ignore Spelling: Deserialize

using Shopway.Domain.Common.Results;

namespace Shopway.Domain.Common.Errors;

/// <summary>
/// Represents an error that contains the informations about the failure
/// </summary>
public sealed partial record class Error(string Code, string Message)
{
    private const string SerializationSeparator = ": ";
    private const int MaximalErrorMessageLenght = 500;

    public string Code { get; } = Code;
    public string Message { get; } = KeepMessageNotTooLongForSecurityReasons(Message);

    /// <summary>
    /// The empty error instance used to represent that no error has occurred
    /// </summary>
    public static readonly Error None = new(string.Empty, string.Empty);

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

    public static implicit operator string(Error error)
    {
        return error.Code;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Message;
    }

    public ValidationResult<TValue> ToValidationResult<TValue>()
    {
        return ValidationResult<TValue>.WithErrors(this);
    }

    public ValidationResult ToValidationResult()
    {
        return ValidationResult.WithErrors(this);
    }

    public Result<TValue> ToResult<TValue>()
    {
        return Result.Failure<TValue>(this);
    }

    public Result ToResult()
    {
        return Result.Failure(this);
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

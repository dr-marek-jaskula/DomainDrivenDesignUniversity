using Microsoft.IdentityModel.Tokens;
using Shopway.Application.Abstractions;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Errors;

namespace Shopway.Infrastructure.Validators;

public sealed class Validator : IValidator
{
    private readonly List<Error> _errors = [];
    public bool IsValid => _errors.IsNullOrEmpty();
    public bool IsInvalid => !IsValid;

    /// <summary>
    /// If the provided condition is true, then error will be added to the error list
    /// </summary>
    /// <param name="condition">Condition for invalid state</param>
    /// <param name="thenError">Error that will be added to the error list if condition is true</param>
    /// <returns>IValidator to chain validation</returns>
    public IValidator If(bool condition, Error thenError)
    {
        if (condition is true)
        {
            _errors.Add(thenError);
        }

        return this;
    }

    /// <summary>
    /// If the result is failure, then result error will be added to the error list
    /// </summary>
    /// <typeparam name="TType">ValueObject type</typeparam>
    /// <param name="result">Result containing the value object</param>
    /// <returns>IValidator to chain validation</returns>
    public IValidator Validate<TType>(Result<TType> result)
    {
        if (result.IsFailure)
        {
            _errors.Add(result.Error);
        }

        return this;
    }

    /// <summary>
    /// If the validation result is failure, then all validation result errors will be added to the error list
    /// </summary>
    /// <param name="validationResult">ValidationResult containing the value object</param>
    /// <returns>IValidator to chain validation</returns>
    public IValidator Validate<TType>(ValidationResult<TType> validationResult)
    {
        if (validationResult.IsFailure)
        {
            _errors.AddRange(validationResult.ValidationErrors);
        }

        return this;
    }

    /// <summary>
    /// Builds the failure validation result with errors
    /// </summary>
    /// <typeparam name="TResponse">Type of response</typeparam>
    /// <returns>ValidationResult with errors</returns>
    /// <exception cref="InvalidOperationException">If validator error list is null or empty, throw exception</exception>
    public ValidationResult<TResponse> Failure<TResponse>()
        where TResponse : IResponse
    {
        if (IsValid)
        {
            throw new InvalidOperationException("Validation was successful, but Failure was called");
        }

        return ValidationResult<TResponse>.WithErrors(_errors.ToArray());
    }

    /// <summary>
    /// Builds the failure validation result with errors
    /// </summary>
    /// <typeparam name="TResponse">Type of response</typeparam>
    /// <returns>ValidationResult with errors</returns>
    /// <exception cref="InvalidOperationException">If validator error list is null or empty, throw exception</exception>
    public ValidationResult Failure()
    {
        if (IsValid)
        {
            throw new InvalidOperationException("Validation was successful, but Failure was called");
        }

        return ValidationResult.WithErrors(_errors.ToArray());
    }
}
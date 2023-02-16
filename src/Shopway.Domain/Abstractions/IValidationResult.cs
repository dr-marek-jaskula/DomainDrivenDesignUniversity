using Shopway.Domain.Errors;

namespace Shopway.Domain.Abstractions;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        code: "ValidationError",
        message: "A validation problem occurred.");

    Error[] ValidationErrors { get; }
}

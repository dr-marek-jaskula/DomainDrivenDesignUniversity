using Shopway.Domain.Errors;

namespace Shopway.Domain.Results;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        code: "ValidationError",
        message: "A validation problem occurred.");

    Error[] Errors { get; }
}

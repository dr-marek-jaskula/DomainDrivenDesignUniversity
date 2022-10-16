using Shopway.Domain.Errors;

namespace Shopway.Domain.Results;

public interface IValidationResult
{
    public static readonly Error ValidationError = new(
        "ValidationError",
        "A validation problem occurred.");

    Error[] Errors { get; }
}

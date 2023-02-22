using Shopway.Domain.Errors;

namespace Shopway.Domain.Abstractions;

public interface IValidationResult
{
    Error[] ValidationErrors { get; }
}

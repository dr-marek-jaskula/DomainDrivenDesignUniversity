using Shopway.Domain.Errors;

namespace Shopway.Domain.Common.Results.Abstractions;

public interface IValidationResult
{
    Error[] ValidationErrors { get; }
}

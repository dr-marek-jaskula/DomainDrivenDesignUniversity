using Shopway.Domain.Common.Errors;

namespace Shopway.Domain.Common.Results.Abstractions;

public interface IValidationResult
{
    Error[] ValidationErrors { get; }
}

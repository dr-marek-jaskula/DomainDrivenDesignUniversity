using Shopway.Domain.Errors;

namespace Shopway.Tests.Integration.ControllersUnderTest;

/// <summary>
/// Represents a helper class used to deserialize the validation problem details
/// </summary>
public sealed class ValidationProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public List<Error> Errors { get; set; } = [];
}
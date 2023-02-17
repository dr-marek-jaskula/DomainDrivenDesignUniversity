using Shopway.Domain.Errors;

namespace Shopway.Tests.Integration.ControllersUnderTest;

public sealed class ValidationProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Detail { get; set; } = string.Empty;
    public List<Error> Errors { get; set; } = new();
}
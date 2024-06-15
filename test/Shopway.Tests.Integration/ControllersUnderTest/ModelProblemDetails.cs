namespace Shopway.Tests.Integration.ControllersUnderTest;

/// <summary>
/// Represents a helper class used to deserialize the model problem details
/// </summary>
public sealed class ModelProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public List<string> Errors { get; set; } = [];
}

namespace Shopway.Tests.Integration.ControllersUnderTest;

public sealed class ModelProblemDetails
{
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public List<string> Errors { get; set; } = new();
}
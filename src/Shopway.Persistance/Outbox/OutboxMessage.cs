namespace Shopway.Persistence.Outbox;

public sealed class OutboxMessage
{
    public required Guid Id { get; set; }

    public required string Type { get; set; }

    public required string Content { get; set; }

    public required DateTimeOffset OccurredOn { get; set; }

    public DateTimeOffset? ProcessedOn { get; set; }

    public string? Error { get; set; }

    public void UpdatePostProcessProperties(DateTimeOffset? processedOn, string? error)
    {
        ProcessedOn = processedOn;
        Error = error;
    }
}
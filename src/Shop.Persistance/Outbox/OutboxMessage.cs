namespace Shopway.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }

    public string Type { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset OccurredOn { get; set; }

    public DateTimeOffset? ProcessedOn { get; set; }

    public string? Error { get; set; }
}
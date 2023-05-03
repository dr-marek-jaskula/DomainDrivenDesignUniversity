namespace Shopway.Persistence.Outbox;

public sealed class OutboxMessageConsumer
{
    public required Guid Id { get; set; }

    public required string Name { get; set; }
}
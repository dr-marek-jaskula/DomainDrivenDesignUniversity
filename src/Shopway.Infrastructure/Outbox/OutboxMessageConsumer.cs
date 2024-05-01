namespace Shopway.Infrastructure.Outbox;

public sealed class OutboxMessageConsumer
{
    public required Ulid Id { get; set; }

    public required string Name { get; set; }
}
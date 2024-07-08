using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Text.Json;

namespace Shopway.Infrastructure.Outbox;

public static class OutboxMessageUtilities
{
    private static readonly Type[] _domainTypes = Domain.AssemblyReference.Assembly.GetTypes();

    public static IDomainEvent? Deserialize(this OutboxMessage outboxMessage)
    {
        if (outboxMessage is null)
        {
            return null;
        }

        var domainEventType = Array.Find(_domainTypes, x => x.Name == outboxMessage.Type);

        if (domainEventType is null)
        {
            return null;
        }

        return (IDomainEvent)JsonSerializer.Deserialize(outboxMessage.Content, domainEventType)!;
    }
}

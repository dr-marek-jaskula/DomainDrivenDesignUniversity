using Shopway.Domain.Common.BaseTypes.Abstractions;
using System.Text.Json;

namespace Shopway.Infrastructure.Outbox;

public static class OutboxMessageUtilities
{
    public static IDomainEvent? Deserialize(this OutboxMessage outboxMessage)
    {
        if (outboxMessage is null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<IDomainEvent>(outboxMessage.Content);
    }
}

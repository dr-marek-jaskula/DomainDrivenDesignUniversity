using Shopway.Domain.DomainEvents;

namespace Shopway.Domain.Primitives;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
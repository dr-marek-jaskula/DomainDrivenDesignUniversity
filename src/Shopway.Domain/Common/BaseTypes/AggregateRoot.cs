using Shopway.Domain.Common.BaseTypes.Abstractions;

namespace Shopway.Domain.Common.BaseTypes;

/// <summary>
/// AggregateRoot is an Entity or a group of Entities that will be allowed to be directly queried from the database.
/// AggragateRoot is always queried as a whole. Entities that are not AggrageteRoot will be queried only as a part of a AggregateRoot
/// </summary>
/// <typeparam name="TEntityId">Type of entity id</typeparam>
public abstract class AggregateRoot<TEntityId> : Entity<TEntityId>, IAggregateRoot
    where TEntityId : struct, IEntityId<TEntityId>
{
    //This allow us to rise the different type of DomainEvents (it is only for AggregateRoots)
    private readonly List<IDomainEvent> _domainEvents = [];

    protected AggregateRoot(TEntityId id)
        : base(id)
    {
    }

    protected AggregateRoot()
    {
    }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    //The way to rise the domain events in the system
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}

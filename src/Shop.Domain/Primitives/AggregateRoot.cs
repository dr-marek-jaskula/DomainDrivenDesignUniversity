namespace Shopway.Domain.Primitives;

//The AggregateRoot is an Entity or a group of Entities that will be allowed to be queried from the database directly.
//AggragateRoot is always queried as a whole.
//Entities that are not AggrageteRoot will be queried only as a part of a AggregateRoot
//The example AggregateRoot is the Order, and the Entity is the Payment (we cannot query the Payment without the Order).
public abstract class AggregateRoot : Entity
{
    //This allow us to rise the different type of DomainEvents (it is only for AggregateRoots)
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(Guid id)
        : base(id)
    {
    }

    protected AggregateRoot()
    {
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

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

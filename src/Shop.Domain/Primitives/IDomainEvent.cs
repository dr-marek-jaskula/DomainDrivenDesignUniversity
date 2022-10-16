using MediatR;

namespace Shopway.Domain.Primitives;

//See Shopway.Domain -> DomainEvents -> DomainEvent
public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
using MediatR;

namespace Shopway.Domain.DomainEvents;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
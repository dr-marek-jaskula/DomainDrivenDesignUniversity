using MediatR;

namespace Shopway.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}
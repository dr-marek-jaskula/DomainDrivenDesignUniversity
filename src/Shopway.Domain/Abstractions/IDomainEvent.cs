using MediatR;

namespace Shopway.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Ulid Id { get; init; }
}
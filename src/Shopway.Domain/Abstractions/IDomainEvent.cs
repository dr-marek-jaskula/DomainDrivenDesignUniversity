using MediatR;

namespace Shopway.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid Id { get; init; }
}
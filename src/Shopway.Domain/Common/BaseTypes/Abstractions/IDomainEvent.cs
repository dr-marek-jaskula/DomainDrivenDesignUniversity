using MediatR;

namespace Shopway.Domain.Common.BaseTypes.Abstractions;

public interface IDomainEvent : INotification
{
    Ulid Id { get; init; }
}

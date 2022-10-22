namespace Shopway.Domain.DomainEvents;

public sealed record EmployeeRegisteredDomainEvent(Guid Id, Guid UserId) : DomainEvent(Id);
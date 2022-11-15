namespace Shopway.Domain.DomainEvents;

public sealed record EmployeeRegisteredDomainEvent(Guid Id, Guid EmployeeId) : DomainEvent(Id);
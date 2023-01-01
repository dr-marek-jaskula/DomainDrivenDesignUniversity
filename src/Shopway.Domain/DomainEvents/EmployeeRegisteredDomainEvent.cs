using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record EmployeeRegisteredDomainEvent(Guid Id, PersonId EmployeeId) : DomainEvent(Id);
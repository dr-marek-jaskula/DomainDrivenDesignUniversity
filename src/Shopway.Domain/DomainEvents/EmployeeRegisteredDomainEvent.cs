using Shopway.Domain.BaseTypes;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Domain.DomainEvents;

public sealed record EmployeeRegisteredDomainEvent(Guid Id, PersonId EmployeeId) : DomainEvent(Id)
{
    public static EmployeeRegisteredDomainEvent New(PersonId EmployeeId)
    {
        return new EmployeeRegisteredDomainEvent(Guid.NewGuid(), EmployeeId);
    }
}
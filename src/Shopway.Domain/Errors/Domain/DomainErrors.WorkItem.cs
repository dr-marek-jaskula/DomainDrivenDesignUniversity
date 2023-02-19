using Shopway.Domain.Entities.Parents;

namespace Shopway.Domain.Errors.Domain;

public static partial class DomainErrors
{
    public static class WorkItemError
    {
        public static readonly Error AlreadyAssigned = new(
            $"{nameof(WorkItem)}.{nameof(AlreadyAssigned)}",
            $"Can't assign the same {nameof(WorkItem)} twice");
    }
}
using Shopway.Domain.Entities.Parents;

namespace Shopway.Domain.Entities;

public class WorkTask : WorkItem
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }
}
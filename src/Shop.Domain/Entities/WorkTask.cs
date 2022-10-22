using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class WorkTask : WorkItem
{
    public WorkTask(
        Guid id, 
        int priority, 
        Status status, 
        string title, 
        Description description, 
        DateTime? startDate, 
        DateTime? endDate, 
        int? employeeId) 
        : base(id, priority, status, title, description)
    {
        StartDate = startDate;
        EndDate = endDate;
        EmployeeId = employeeId;
    }

    // Empty constructor in this case is required by EF Core
    public WorkTask()
    {
    }

    public DateTime? StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public int? EmployeeId { get; private set; }
    public Employee? Employee { get; private set; }
}
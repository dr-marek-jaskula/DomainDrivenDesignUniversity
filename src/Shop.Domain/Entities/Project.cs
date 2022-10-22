using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Project : WorkItem
{
    public Project(
        Guid id, 
        int priority, 
        Status status, 
        string title, 
        Description description, 
        Employee? projectLeader, 
        int projectLeaderId) 
        : base(id, priority, status, title, description)
    {
        ProjectLeader = projectLeader;
        ProjectLeaderId = projectLeaderId;
    }

    // Empty constructor in this case is required by EF Core
    private Project()
    {
    }

    public Employee? ProjectLeader { get; private set; }
    public int ProjectLeaderId { get; private set; }
}
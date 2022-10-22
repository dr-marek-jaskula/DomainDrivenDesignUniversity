using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;

namespace Shopway.Domain.Entities;

public sealed class Project : WorkItem
{
    public Project(
        Guid id, 
        int priority, 
        Status status, 
        string title, 
        string description, 
        Employee? projectLeader, 
        int projectLeaderId) 
        : base(id, priority, status, title, description)
    {
        ProjectLeader = projectLeader;
        ProjectLeaderId = projectLeaderId;
    }

    public Employee? ProjectLeader { get; private set; }
    public int ProjectLeaderId { get; private set; }
}
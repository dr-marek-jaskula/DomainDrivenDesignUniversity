using Shopway.Domain.DomainEvents;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.ValueObjects;

namespace Shopway.Domain.Entities;

public sealed class Feature : WorkItem
{
    public Feature(
        Guid id,
        Title title,
        Description description,
        Priority priority,
        StoryPoints storyPoints,
        Status status,
        Guid? employeeId)
        : base(id, title, description, priority, storyPoints, status, employeeId)
    {
    }

    // Empty constructor in this case is required by EF Core
    public Feature()
    {
    }

    public static Feature Create(
        Guid id,
        Title title,
        Description description,
        Priority priority,
        StoryPoints storyPoints,
        Status status,
        Guid? employeeId)
    {
        var feature = new Feature(
            id,
            title,
            description,
            priority,
            storyPoints,
            status,
            employeeId);

        feature.RaiseDomainEvent(new FeatureRegisteredDomainEvent(Guid.NewGuid(), feature.Id));

        return feature;
    }
}
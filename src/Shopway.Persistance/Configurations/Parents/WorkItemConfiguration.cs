using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Persistence.Converters;

namespace Shopway.Persistence.Configurations.Parents;

internal class WorkItemEntityTypeConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable(TableNames.WorkItem);

        builder.HasKey(wi => wi.Id);

        builder.Property(wi => wi.Id)
            .HasConversion(id => id.Value, guid => WorkItemId.New(guid))
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(wi => wi.Priority)
            .HasConversion(x => x.Value, v => Priority.Create(v).Value)
            .HasDefaultValue(Priority.Create(Priority.HighestPriority).Value);

        builder.Property(wi => wi.Status)
            .IsRequired(true)
            .HasColumnType("VARCHAR(10)")
            .HasDefaultValue(Status.New)
            .HasConversion(status => status.ToString(),
             s => (Status)Enum.Parse(typeof(Status), s))
            .HasComment("New, InProgress, Done or Rejected");

        builder.Property(wi => wi.Title)
            .HasConversion(x => x.Value, v => Title.Create(v).Value)
            .HasMaxLength(Title.MaxLength);

        builder.Property(wi => wi.Description)
            .HasConversion(x => x.Value, v => Description.Create(v).Value)
            .HasMaxLength(Description.MaxLength);

        builder.Property(wi => wi.StoryPoints)
            .HasConversion(x => x.Value, v => StoryPoints.Create(v).Value)
            .HasDefaultValue(StoryPoints.Create(StoryPoints.MinStoryPoints).Value);

        builder.HasOne(wi => wi.Employee)
            .WithMany(e => e.WorkItems)
            .HasForeignKey(wi => wi.EmployeeId);
    }
}
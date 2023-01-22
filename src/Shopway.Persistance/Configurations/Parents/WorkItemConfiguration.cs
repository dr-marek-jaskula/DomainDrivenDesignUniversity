using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Configurations.Parents;

internal class WorkItemEntityTypeConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable(TableNames.WorkItem, SchemaNames.Workflow);

        builder.HasKey(wi => wi.Id);

        builder.Property(wi => wi.Id)
            .HasConversion(id => id.Value, guid => WorkItemId.Create(guid))
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.Status)
            .IsRequired(true)
            .HasColumnType(ColumnTypes.VarChar(10))
            .HasConversion(status => status.ToString(), s => (Status)Enum.Parse(typeof(Status), s));

        builder
            .OwnsOne(p => p.Priority, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Priority))
                    .HasColumnType(ColumnTypes.TinyInt)
                    .HasDefaultValue(Priority.HighestPriority)
                    .IsRequired(true);
            });

        builder
            .OwnsOne(p => p.Title, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Title))
                    .IsRequired(true)
                    .HasMaxLength(Title.MaxLength);
            });

        builder
            .OwnsOne(p => p.Description, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Description))
                    .IsRequired(true)
                    .HasMaxLength(Description.MaxLength);
            });

        builder
            .OwnsOne(p => p.StoryPoints, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(StoryPoints))
                    .HasColumnType(ColumnTypes.TinyInt)
                    .HasDefaultValue(StoryPoints.MinStoryPoints)
                    .IsRequired(true);
            });

        builder.HasOne(wi => wi.Employee)
            .WithMany(e => e.WorkItems)
            .HasForeignKey(wi => wi.EmployeeId);
    }
}
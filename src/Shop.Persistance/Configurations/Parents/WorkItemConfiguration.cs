using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities.Parents;
using Shopway.Domain.Enums;

namespace Shopway.Persistence.Configurations.Parents;

public class WorkItemEntityTypeConfiguration : IEntityTypeConfiguration<WorkItem>
{
    public void Configure(EntityTypeBuilder<WorkItem> builder)
    {
        builder.ToTable("WorkItem");

        builder.HasKey(wi => wi.Id);
        builder.Property(wi => wi.Id)
            .HasColumnType("uniqueidentifier");

        builder.Property(wi => wi.Priority)
            .HasDefaultValue(1);

        builder.Property(wi => wi.Status)
            .IsRequired(true)
            .HasColumnType("VARCHAR(10)")
            .HasDefaultValue(Status.Received)
            .HasConversion(status => status.ToString(),
             s => (Status)Enum.Parse(typeof(Status), s))
            .HasComment("Received, InProgress, Done or Rejected");

        builder.Property(wi => wi.Title)
            .HasColumnType("VARCHAR(45)")
            .IsRequired(true);

        builder.Property(wi => wi.Description)
            .HasColumnType("VARCHAR(600)")
            .IsRequired(true);
    }
}
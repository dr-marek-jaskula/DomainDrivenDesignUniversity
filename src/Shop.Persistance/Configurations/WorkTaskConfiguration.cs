using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public class TaskEntityTypeConfiguration : IEntityTypeConfiguration<WorkTask>
{
    public void Configure(EntityTypeBuilder<WorkTask> builder)
    {
        builder.Property(wi => wi.EndDate)
            .HasPrecision(0);

        builder.Property(wi => wi.StartDate)
            .HasPrecision(0);

        builder.HasOne(wi => wi.Employee)
            .WithOne(e => e.CurrentTask)
            .HasForeignKey<WorkTask>(e => e.EmployeeId);
    }
}
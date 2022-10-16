using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public sealed class EmployeeEntityTypeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employee");

        builder.Property(e => e.HireDate)
            .HasColumnType("DATE")
            .HasDefaultValue(null);

        //Defining the relations: (rest are in Customer and Review classes)

        builder.HasOne(e => e.Salary)
            .WithOne(s => s.Employee)
            .HasForeignKey<Employee>(e => e.SalaryId);

        builder.HasOne(e => e.Manager)
            .WithMany(x => x.Subordinates)
            .HasForeignKey(e => e.ManagerId)
            .IsRequired(false);
    }
}
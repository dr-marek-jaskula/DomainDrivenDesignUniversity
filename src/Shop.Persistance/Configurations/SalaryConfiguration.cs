using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public sealed class SalaryEntityTypeConfiguration : IEntityTypeConfiguration<Salary>
{
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
        builder.ToTable("Salary");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("SMALLINT").UseIdentityColumn();

        builder.Property(s => s.BaseSalary)
            .HasDefaultValue(0)
            .HasColumnType("INT");

        builder.Property(s => s.DiscretionaryBonus)
            .HasDefaultValue(0)
            .HasColumnType("INT");

        builder.Property(s => s.IncentiveBonus)
            .HasDefaultValue(0)
            .HasColumnType("INT");

        builder.Property(s => s.TaskBonus)
            .HasDefaultValue(0)
            .HasColumnType("INT");

        builder.HasMany(s => s.SalaryTransfer)
            .WithOne(st => st.Salary)
            .HasForeignKey(s => s.SalaryId);
    }
}
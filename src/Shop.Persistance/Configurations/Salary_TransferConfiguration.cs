using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public class SalaryTransferEntityTypeConfiguration : IEntityTypeConfiguration<Salary_Transfer>
{
    public void Configure(EntityTypeBuilder<Salary_Transfer> builder)
    {
        builder.ToTable("Salary_Transfer");

        builder.Property(s => s.Id).UseIdentityColumn();

        builder.Property(s => s.Date)
            .IsRequired(true)
            .HasColumnType("DATE");

        builder.Property(s => s.IsTaskBonus)
            .HasDefaultValue(false)
            .HasColumnType("BIT");

        builder.Property(s => s.IsDiscretionaryBonus)
            .HasDefaultValue(false)
            .HasColumnType("BIT");

        builder.Property(s => s.IsIncentiveBonus)
            .HasDefaultValue(false)
            .HasColumnType("BIT");

        builder.Property(s => s.SalaryId)
            .HasColumnType("SMALLINT");
    }
}
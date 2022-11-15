using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable(TableNames.Customer);

        builder.Property(c => c.Rank)
            .HasDefaultValue(Rank.Standard)
            .HasColumnType("VARCHAR(8)")
            .HasConversion(r => r.ToString(),
            s => (Rank)Enum.Parse(typeof(Rank), s));
    }
}
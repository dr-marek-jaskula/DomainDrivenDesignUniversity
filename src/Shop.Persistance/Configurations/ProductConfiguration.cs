using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Product);

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(p => p.ProductName)
            .IsRequired(true)
            .HasMaxLength(128);

        builder.Property(p => p.Price)
            .IsRequired(true)
            .HasPrecision(10, 2);
    }
}
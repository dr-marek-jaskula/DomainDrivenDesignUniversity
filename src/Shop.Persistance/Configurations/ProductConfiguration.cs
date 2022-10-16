using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Product");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();

        builder.Property(p => p.Name)
            .IsRequired(true)
            .HasMaxLength(128);

        builder.Property(p => p.Price)
            .IsRequired(true)
            .HasPrecision(10, 2);
    }
}
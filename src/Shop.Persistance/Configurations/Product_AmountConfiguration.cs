using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public class ProductAmountEntityTypeConfiguration : IEntityTypeConfiguration<Product_Amount>
{
    public void Configure(EntityTypeBuilder<Product_Amount> builder)
    {
        builder.ToTable("Product_Amount");

        builder.HasKey(pa => new { pa.ProductId, pa.ShopId });

        builder.Property(pa => pa.Amount)
            .IsRequired(true)
            .HasColumnType("INT");
    }
}
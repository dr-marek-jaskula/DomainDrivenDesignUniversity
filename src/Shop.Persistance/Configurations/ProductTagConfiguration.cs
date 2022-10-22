using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence.Configurations;

public sealed class Product_TagEntityTypeConfiguration : IEntityTypeConfiguration<Product_Tag>
{
    public void Configure(EntityTypeBuilder<Product_Tag> builder)
    {
        builder.ToTable(TableNames.ProductTag);

        builder.HasKey(pt => new { pt.ProductId, pt.TagId });
    }
}
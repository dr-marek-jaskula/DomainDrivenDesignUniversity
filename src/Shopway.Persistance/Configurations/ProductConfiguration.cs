using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Configurations;

internal sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Product);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, guid => ProductId.New(guid))
            .HasColumnType("UNIQUEIDENTIFIER");

        builder
            .Property(p => p.ProductName)
            .HasConversion(x => x.Value, v => ProductName.Create(v).Value)
            .IsRequired(true)
            .HasMaxLength(128);

        builder
            .Property(p => p.Revision)
            .HasConversion(x => x.Value, v => Revision.Create(v).Value)
            .IsRequired(true)
            .HasMaxLength(64);

        builder
            .Property(p => p.UomCode)
            .HasConversion(x => x.Value, v => UomCode.Create(v).Value)
            .IsRequired(true)
            .HasMaxLength(8);

        builder.Property(p => p.Price)
            .HasConversion(x => x.Value, v => Price.Create(v).Value)
            .IsRequired(true)
            .HasPrecision(10, 2);

        builder.HasMany(p => p.Reviews)
            .WithOne()
            .HasForeignKey(r => r.ProductId);
    }
}
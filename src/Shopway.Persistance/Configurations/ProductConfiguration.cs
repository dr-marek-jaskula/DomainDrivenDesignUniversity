using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.EntityIds;

namespace Shopway.Persistence.Configurations;

internal sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Product, SchemaNames.Shopway);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<ProductIdConverter, EntityIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.ConfigureAuditableEntity();

        builder
            .OwnsOne(p => p.ProductName, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(ProductName))
                    .IsRequired(true)
                    .HasMaxLength(ProductName.MaxLength);
            });

        builder
            .OwnsOne(p => p.Revision, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Revision))
                    .IsRequired(true)
                    .HasMaxLength(Revision.MaxLength);
            });

        builder
            .OwnsOne(p => p.UomCode, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(UomCode))
                    .HasColumnType(ColumnTypes.VarChar(8))
                    .IsRequired(true);
            });

        builder
            .OwnsOne(p => p.Price, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Price))
                    .IsRequired(true)
                    .HasPrecision(NumberConstants.DecimalPrecision, NumberConstants.DecimalScale);
            });

        builder.HasMany(p => p.Reviews)
            .WithOne()
            .HasForeignKey(r => r.ProductId);
    }
}
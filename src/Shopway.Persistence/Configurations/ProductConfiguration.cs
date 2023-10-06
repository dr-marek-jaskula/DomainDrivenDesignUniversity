using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.ValueObjects;
using static Shopway.Persistence.Constants.NumberConstants;

namespace Shopway.Persistence.Configurations;

internal sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Product, SchemaNames.Shopway);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<ProductIdConverter, ProductIdComparer>()
            .HasColumnType(ColumnTypes.Char(UlidCharLenght));

        builder.ConfigureAuditableEntity();

        builder.Property(p => p.ProductName)
            .HasConversion<ProductNameConverter, ProductNameComparer>()
            .HasColumnName(nameof(ProductName))
            .HasMaxLength(ProductName.MaxLength)
            .IsRequired(true);

        builder.Property(p => p.Revision)
            .HasConversion<RevisionConverter, RevisionComparer>()
            .HasColumnName(nameof(Revision))
            .HasMaxLength(Revision.MaxLength)
            .IsRequired(true);

        builder.Property(p => p.UomCode)
            .HasConversion<UomCodeConverter, UomCodeComparer>()
            .HasColumnName(nameof(UomCode))
            .HasColumnType(ColumnTypes.VarChar(8))
            .IsRequired(true);

        builder.Property(p => p.Price)
            .HasConversion<PriceConverter, PriceComparer>()
            .HasColumnName(nameof(Price))
            .HasPrecision(DecimalPrecision, DecimalScale)
            .IsRequired(true);

        builder.HasMany(p => p.Reviews)
            .WithOne()
            .HasForeignKey(r => r.ProductId);

        //Indexes
        builder
            .HasIndex(p => new { p.ProductName, p.Revision })
            .HasDatabaseName($"UX_{nameof(Product)}_{nameof(ProductName)}_{nameof(Revision)}")
            .IsUnique();
    }
}
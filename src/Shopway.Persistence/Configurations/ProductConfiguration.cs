using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Persistence.Converters.ValueObjects;
using Shopway.Persistence.Utilities;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;

namespace Shopway.Persistence.Configurations;

[GenerateEntityIdComparer(IdName = ProductId.Name, IdNamespace = ProductId.Namespace)]
[GenerateEntityIdConverter(IdName = ProductId.Name, IdNamespace = ProductId.Namespace)]
internal sealed class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableName.Product, SchemaName.Shopway);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<ProductIdConverter, ProductIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

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
            .HasColumnType(ColumnType.VarChar(8))
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

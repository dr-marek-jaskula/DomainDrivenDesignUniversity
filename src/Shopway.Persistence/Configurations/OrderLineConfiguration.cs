using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Entities;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.ValueObjects;
using Shopway.Persistence.Utilities;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;
using static Shopway.Persistence.Utilities.ConfigurationUtilities;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderLineEntityTypeConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable(TableName.OrderLine, SchemaName.Shopway);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion<OrderLineIdConverter, OrderLineIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.OwnsOne(o => o.ProductSummary, options =>
        {
            options.ToJson(nameof(ProductSummary));

            options.Property(p => p.ProductId)
                .HasConversion<ProductIdConverter, ProductIdComparer>();

            options.Property(x => x.ProductName)
                .HasConversion<ProductNameConverter, ProductNameComparer>()
                .IsRequired(true);

            options.Property(p => p.Revision)
                .HasConversion<RevisionConverter, RevisionComparer>()
                .IsRequired(true);

            options.Property(p => p.Price)
                .HasConversion<PriceConverter, PriceComparer>()
                .IsRequired(true);

            options.Property(p => p.UomCode)
                .HasConversion<UomCodeConverter, UomCodeComparer>()
                .IsRequired(true);
        });

        builder.Property(o => o.OrderHeaderId)
            .HasConversion<OrderHeaderIdConverter, OrderHeaderIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.ConfigureAuditableEntity();

        builder.Property(o => o.Amount)
            .HasConversion<AmountConverter, AmountComparer>()
            .HasColumnName(nameof(Amount))
            .IsRequired(true);

        builder.Property(o => o.LineDiscount)
            .HasConversion<DiscountConverter, DiscountComparer>()
            .HasColumnName(nameof(Discount))
            .HasPrecision(DiscountPrecision, DecimalScale)
            .IsRequired(true);
    }
}
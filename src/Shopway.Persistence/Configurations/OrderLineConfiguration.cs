using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Utilities;
using Shopway.Domain.Entities;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.ValueObjects;
using static Shopway.Persistence.Constants.NumberConstants;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderLineEntityTypeConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable(TableNames.OrderLine, SchemaNames.Shopway);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion<OrderLineIdConverter, OrderLineIdComparer>()
            .HasColumnType(ColumnTypes.Char(UlidCharLenght));

        builder.Property(o => o.ProductId)
            .HasConversion<ProductIdConverter, ProductIdComparer>()
            .HasColumnType(ColumnTypes.Char(UlidCharLenght));

        builder.Property(o => o.OrderHeaderId)
            .HasConversion<OrderHeaderIdConverter, OrderHeaderIdComparer>()
            .HasColumnType(ColumnTypes.Char(UlidCharLenght));

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

        builder.HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId);
    }
}
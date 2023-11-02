using Shopway.Domain.Entities;
using Shopway.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        builder.Property(o => o.ProductId)
            .HasConversion<ProductIdConverter, ProductIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

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

        builder.HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId);
    }
}
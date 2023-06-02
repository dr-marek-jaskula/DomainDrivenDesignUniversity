using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Utilities;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.Enums;
using Shopway.Persistence.Converters.ValueObjects;
using static Shopway.Domain.Utilities.EnumUtilities;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderHeaderEntityTypeConfiguration : IEntityTypeConfiguration<OrderHeader>
{
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.ToTable(TableNames.OrderHeader, SchemaNames.Shopway);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion<OrderHeaderIdConverter, EntityIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.Status)
            .HasConversion<OrderStatusConverter>()
            .HasColumnType(ColumnTypes.VarChar(LongestOf<OrderStatus>()))
            .IsRequired(true);

        builder.Property(p => p.TotalDiscount)
            .HasConversion<DiscountConverter, DiscountComparer>()
            .HasColumnName(nameof(Discount))
            .HasPrecision(NumberConstants.DiscountPrecision, NumberConstants.DecimalScale)
            .IsRequired(true);

        builder.ConfigureAuditableEntity();

        builder.HasOne(p => p.Payment)
            .WithOne(p => p.OrderHeader)
            .HasForeignKey<OrderHeader>(o => o.PaymentId);

        builder.HasMany(o => o.OrderLines)
            .WithOne()
            .HasForeignKey(line => line.OrderHeaderId);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.Enumerations;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.Enums;
using Shopway.Persistence.Converters.ValueObjects;
using Shopway.Persistence.Utilities;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;
using static Shopway.Persistence.Utilities.ConfigurationUtilities;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderHeaderEntityTypeConfiguration : IEntityTypeConfiguration<OrderHeader>
{
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.ToTable(TableName.OrderHeader, SchemaName.Shopway);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion<OrderHeaderIdConverter, OrderHeaderIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.Property(o => o.UserId)
            .HasConversion<UserIdConverter, UserIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.Property(p => p.Status)
            .HasConversion<OrderStatusConverter>()
            .HasColumnType(ColumnType.VarChar(LongestOf<OrderStatus>()))
            .IsRequired(true);

        builder.Property(p => p.TotalDiscount)
            .HasConversion<DiscountConverter, DiscountComparer>()
            .HasColumnName(nameof(Discount))
            .HasPrecision(DiscountPrecision, DecimalScale)
            .IsRequired(true);

        builder
            .ConfigureAuditableEntity()
            .ConfigureSoftDeletableEntity();

        builder.HasMany(p => p.Payments)
            .WithOne()
            .HasForeignKey(payment => payment.OrderHeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.OrderLines)
            .WithOne()
            .HasForeignKey(line => line.OrderHeaderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Orders;
using Shopway.Domain.Orders.Enumerations;
using Shopway.Domain.Orders.ValueObjects;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.Enums;
using Shopway.Persistence.Utilities;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;

namespace Shopway.Persistence.Configurations;

[GenerateEntityIdComparer(IdName = PaymentId.Name, IdNamespace = PaymentId.Namespace)]
[GenerateEntityIdConverter(IdName = PaymentId.Name, IdNamespace = PaymentId.Namespace)]
internal sealed class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(TableName.Payment, SchemaName.Shopway);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<PaymentIdConverter, PaymentIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.Property(p => p.Status)
            .HasConversion<PaymentStatusConverter>()
            .HasColumnType(ColumnType.VarChar(LongestOf<PaymentStatus>()))
            .IsRequired(true);

        builder.Property(o => o.OrderHeaderId)
            .HasConversion<OrderHeaderIdConverter, OrderHeaderIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght))
            .IsRequired(true);

        builder.Property(entity => entity.IsRefunded)
            .HasColumnType(ColumnType.Bit)
            .HasDefaultValue(false);

        builder.OwnsOne(o => o.Session, options =>
        {
            options.ToJson(nameof(Session));

            options.Property(p => p.Id).IsRequired(false);
            options.Property(x => x.Secret).IsRequired(false);
            options.Property(x => x.PaymentIntentId).IsRequired(false);
        });

        builder.ConfigureAuditableEntity();
    }
}
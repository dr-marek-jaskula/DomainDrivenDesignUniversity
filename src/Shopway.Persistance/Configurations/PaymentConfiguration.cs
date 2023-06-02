using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Persistence.Converters.Enums;
using static Shopway.Domain.Utilities.EnumUtilities;

namespace Shopway.Persistence.Configurations;

internal sealed class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(TableNames.Payment, SchemaNames.Shopway);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion<PaymentIdConverter, PaymentIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.OrderHeaderId)
            .HasConversion<OrderHeaderIdConverter, OrderHeaderIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.Status)
            .HasConversion<PaymentStatusConverter>()
            .HasColumnType(ColumnTypes.VarChar(LongestOf<PaymentStatus>()))
            .IsRequired(true);

        builder.ConfigureAuditableEntity();
    }
}
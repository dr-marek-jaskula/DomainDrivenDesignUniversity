using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Utilities;

namespace Shopway.Persistence.Configurations;

internal sealed class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(TableNames.Payment, SchemaNames.Shopway);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, guid => PaymentId.Create(guid))
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.OrderHeaderId)
            .HasConversion(p => p.Value, guid => OrderHeaderId.Create(guid))
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.Status)
            .HasColumnType(ColumnTypes.VarChar(11))
            .HasConversion(status => status.ToString(), s => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), s))
            .IsRequired(true);

        builder.ConfigureAuditableEntity();
    }
}
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

        builder.Property(p => p.OrderId)
            .HasConversion(p => p.Value, guid => OrderId.Create(guid))
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.Status)
            .HasColumnType(ColumnTypes.VarChar(10))
            .HasConversion(status => status.ToString(), s => (Status)Enum.Parse(typeof(Status), s))
            .IsRequired(true);

        builder.ConfigureAuditableEntity();

        builder
            .OwnsOne(p => p.Discount, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Discount))
                    .IsRequired(true)
                    .HasPrecision(NumberConstants.DiscountPrecision, NumberConstants.DecimalScale);
            });

        //Indexes
        builder.HasIndex(o => new { o.OrderId, o.Status }, "IX_Payment_OrderId_Status")
            .HasFilter("Status <> 'Rejected'");
    }
}
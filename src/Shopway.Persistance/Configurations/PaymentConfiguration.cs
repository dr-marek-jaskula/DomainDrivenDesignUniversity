using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Configurations;

internal sealed class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable(TableNames.Payment);

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasConversion(id => id.Value, guid => PaymentId.New(guid))
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(p => p.Discount)
            .HasConversion(x => x.Value, v => Discount.Create(v).Value)
            .HasPrecision(3, 2);

        builder.Property(p => p.OrderId)
            .HasConversion(p => p.Value, p => new OrderId() { Value = p })
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(p => p.Status)
            .IsRequired(true)
            .HasMaxLength(10)
            .HasConversion(status => status.ToString(),
            s => (Status)Enum.Parse(typeof(Status), s))
            .HasComment("New, InProgress, Done or Rejected");

        builder.Property(p => p.OccurredOn)
            .HasColumnType("datetimeoffset(2)")
            .IsRequired(false);

        //Indexes
        builder.HasIndex(o => new { o.OrderId, o.Status }, "IX_Payment_OrderId_Status")
            .HasFilter("Status <> 'Rejected'");
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;

namespace Shopway.Persistence.Configurations;

public class PaymentEntityTypeConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("Payment");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).UseIdentityColumn();

        builder.Property(p => p.Discount)
            .HasPrecision(3, 2);

        builder.Property(p => p.Total)
            .IsRequired(true)
            .HasPrecision(12, 2);

        builder.Property(p => p.Status)
            .IsRequired(true)
            .HasMaxLength(10)
            .HasConversion(status => status.ToString(),
            s => (Status)Enum.Parse(typeof(Status), s))
            .HasComment("Received, InProgress, Done or Rejected");

        builder.Property(p => p.Deadline)
            .HasColumnType("DATE");

        builder.HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Order>(o => o.PaymentId);

        //Indexes
        builder.HasIndex(o => new { o.Deadline, o.Status }, "IX_Payment_Deadline_Status")
            .IncludeProperties(o => o.Total)
            .HasFilter("Status <> 'Rejected'");
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(TableNames.Order);

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasConversion(p => p.Value, p => new OrderId() { Value = p })
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(o => o.Amount)
            .IsRequired(true)
            .HasConversion(x => x.Value, v => Amount.Create(v).Value)
            .HasColumnType("INT");

        builder.Property(o => o.Status)
            .IsRequired(true)
            .HasColumnType("VARCHAR(10)")
            .HasDefaultValue(Status.New)
            .HasConversion(status => status.ToString(),
             s => (Status)Enum.Parse(typeof(Status), s))
            .HasComment("New, InProgress, Done or Rejected");

        builder.Property(o => o.CreatedOn)
            .HasColumnType("datetimeoffset(2)");

        builder.Property(o => o.UpdatedOn)
            .HasColumnType("datetimeoffset(2)");

        builder.HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId);

        builder.HasOne(o => o.Customer)
            .WithMany(p => p.Orders)
            .HasForeignKey(p => p.CustomerId);

        builder.HasOne(p => p.Payment)
            .WithOne()
            .HasForeignKey<Order>(o => o.PaymentId);

        //Indexes
        builder.HasIndex(o => new { o.ProductId, o.Status }, "IX_Order_ProductId_Status")
            .IncludeProperties(o => new { o.Amount, o.CustomerId })
            .HasFilter("Status IN ('New', 'InProgress')");
    }
}
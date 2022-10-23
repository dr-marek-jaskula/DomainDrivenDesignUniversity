using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(TableNames.Order);

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(o => o.Amount)
            .IsRequired(true)
            .HasColumnType("INT");

        builder.Property(o => o.Status)
            .IsRequired(true)
            .HasColumnType("VARCHAR(10)")
            .HasDefaultValue(Status.New)
            .HasConversion(status => status.ToString(),
             s => (Status)Enum.Parse(typeof(Status), s))
            .HasComment("New, InProgress, Done or Rejected");

        builder.Property(o => o.Deadline)
            .HasColumnType("DATE");

        builder.HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .HasForeignKey(p => p.ProductId);

        builder.HasOne(o => o.Customer)
            .WithMany(p => p.Orders)
            .HasForeignKey(p => p.CustomerId);

        //Indexes
        builder.HasIndex(o => new { o.Deadline, o.Status }, "IX_Order_Deadline_Status")
            .IncludeProperties(o => new { o.Amount, o.ProductId })
            .HasFilter("Status IN ('Received', 'InProgress')");
    }
}
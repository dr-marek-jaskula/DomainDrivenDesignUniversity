using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Utilities;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(TableNames.Order, SchemaNames.Shopway);

        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
            .HasConversion(id => id.Value, guid => OrderId.Create(guid))
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.Status)
            .IsRequired(true)
            .HasColumnType(ColumnTypes.VarChar(10))
            .HasConversion(status => status.ToString(), s => (Status)Enum.Parse(typeof(Status), s));

        builder.ConfigureAuditableEntity();

        builder
            .OwnsOne(p => p.Amount, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Amount))
                    .IsRequired(true);
            });

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
        builder.HasIndex(o => new { o.ProductId, o.Status })
            .HasDatabaseName($"IX_{nameof(Order)}_{nameof(ProductId)}_{nameof(Status)}")
            //.IncludeProperties(o => new { o.Amount, o.CustomerId })
            .HasFilter("Status IN ('New', 'InProgress')");
    }
}
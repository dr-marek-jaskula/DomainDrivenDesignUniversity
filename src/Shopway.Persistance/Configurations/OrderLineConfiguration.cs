using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Utilities;
using Shopway.Domain.Entities;
using Shopway.Persistence.Converters.EntityIds;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderLineEntityTypeConfiguration : IEntityTypeConfiguration<OrderLine>
{
    public void Configure(EntityTypeBuilder<OrderLine> builder)
    {
        builder.ToTable(TableNames.OrderLine, SchemaNames.Shopway);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion<OrderLineIdConverter>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.ConfigureAuditableEntity();

        builder
            .OwnsOne(p => p.Amount, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Amount))
                    .IsRequired(true);
            });

        builder
            .OwnsOne(p => p.LineDiscount, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Discount))
                    .IsRequired(true)
                    .HasPrecision(NumberConstants.DiscountPrecision, NumberConstants.DecimalScale);
            });

        builder.HasOne(o => o.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductId);
    }
}
﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Utilities;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Converters;

namespace Shopway.Persistence.Configurations;

internal sealed class OrderHeaderEntityTypeConfiguration : IEntityTypeConfiguration<OrderHeader>
{
    public void Configure(EntityTypeBuilder<OrderHeader> builder)
    {
        builder.ToTable(TableNames.OrderHeader, SchemaNames.Shopway);

        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasConversion<OrderHeaderIdConverter>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(p => p.Status)
            .IsRequired(true)
            .HasColumnType(ColumnTypes.VarChar(10))
            .HasConversion(status => status.ToString(), s => (OrderStatus)Enum.Parse(typeof(OrderStatus), s));

        builder
            .OwnsOne(p => p.TotalDiscount, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Discount))
                    .IsRequired(true)
                    .HasPrecision(NumberConstants.DiscountPrecision, NumberConstants.DecimalScale);
            });

        builder.ConfigureAuditableEntity();

        builder.HasOne(p => p.Payment)
            .WithOne(p => p.OrderHeader)
            .HasForeignKey<OrderHeader>(o => o.PaymentId);

        builder.HasMany(o => o.OrderLines)
            .WithOne()
            .HasForeignKey(line => line.OrderHeaderId);
    }
}
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence.Configurations;

public sealed class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable(TableNames.Tag);

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(p => p.ProductTag)
            .IsRequired(true)
            .HasColumnType("VARCHAR(9)")
            .HasConversion(pt => pt.ToString(),
            s => (ProductTag)Enum.Parse(typeof(ProductTag), s));
    }
}
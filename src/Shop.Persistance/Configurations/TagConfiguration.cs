using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.Enums;

namespace Shopway.Persistence.Configurations;

public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("Tag");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).HasColumnType("SMALLINT").UseIdentityColumn();

        builder.Property(p => p.ProductTag)
            .IsRequired(true)
            .HasColumnType("VARCHAR(9)")
            .HasConversion(pt => pt.ToString(),
            s => (ProductTag)Enum.Parse(typeof(ProductTag), s));
    }
}
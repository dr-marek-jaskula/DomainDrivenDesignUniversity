using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.EntityIds;

namespace Shopway.Persistence.Configurations;

internal sealed class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(TableNames.Review, SchemaNames.Shopway);

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value, guid => ReviewId.Create(guid))
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(r => r.ProductId)
            .HasConversion(id => id.Value, guid => ProductId.Create(guid))
            .HasColumnType(ColumnTypes.UniqueIdentifier)
            .IsRequired(true);

        builder.Property(u => u.CreatedOn)
            .HasColumnType(ColumnTypes.DateTimeOffset(2))
            .IsRequired(true);

        builder.Property(u => u.UpdatedOn)
            .HasColumnType(ColumnTypes.DateTimeOffset(2))
            .IsRequired(false);

        builder
            .OwnsOne(p => p.Username, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Username))
                    .IsRequired(true)
                    .HasMaxLength(Username.MaxLength);
            });

        builder
            .OwnsOne(p => p.Title, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Title))
                    .IsRequired(true)
                    .HasMaxLength(Title.MaxLength);
            });

        builder
            .OwnsOne(p => p.Stars, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Stars))
                    .HasColumnType(ColumnTypes.TinyInt)
                    .IsRequired(true);
            });

        builder
            .OwnsOne(p => p.Description, navigationBuilder =>
            {
                navigationBuilder
                    .Property(n => n.Value)
                    .HasColumnName(nameof(Description))
                    .IsRequired(true)
                    .HasMaxLength(Description.MaxLength);
            });
    }
}
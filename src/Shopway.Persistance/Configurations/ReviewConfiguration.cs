using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.EntityIds;

namespace Shopway.Persistence.Configurations;

internal sealed class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(TableNames.Review, SchemaNames.Shopway);

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasConversion<ReviewIdConverter, EntityIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(r => r.ProductId)
            .HasConversion<ProductIdConverter>()
            .HasColumnType(ColumnTypes.UniqueIdentifier)
            .IsRequired(true);

        builder.ConfigureAuditableEntity();

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
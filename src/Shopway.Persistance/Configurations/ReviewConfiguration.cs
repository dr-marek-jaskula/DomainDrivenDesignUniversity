using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Utilities;
using Shopway.Persistence.Converters.EntityIds;
using Shopway.Domain.EntityIds;
using Shopway.Persistence.Converters.ValueObjects;

namespace Shopway.Persistence.Configurations;

internal sealed class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(TableNames.Review, SchemaNames.Shopway);

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasConversion<ReviewIdConverter, ReviewIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier);

        builder.Property(r => r.ProductId)
            .HasConversion<ProductIdConverter, ProductIdComparer>()
            .HasColumnType(ColumnTypes.UniqueIdentifier)
            .IsRequired(true);

        builder.ConfigureAuditableEntity();

        builder.Property(r => r.Username)
            .HasConversion<UsernameConverter, UsernameComparer>()
            .HasColumnName(nameof(Username))
            .HasMaxLength(Username.MaxLength)
            .IsRequired(true);

        builder.Property(r => r.Title)
            .HasConversion<TitleConverter, TitleComparer>()
            .HasColumnName(nameof(Title))
            .HasMaxLength(Title.MaxLength)
            .IsRequired(true);

        builder.Property(r => r.Stars)
            .HasConversion<StarsConverter, StarsComparer>()
            .HasColumnName(nameof(Stars))
            .HasColumnType(ColumnTypes.TinyInt)
            .IsRequired(true);

        builder.Property(r => r.Description)
            .HasConversion<DescriptionConverter, DescriptionComparer>()
            .HasColumnName(nameof(Description))
            .HasMaxLength(Description.MaxLength)
            .IsRequired(true);

        //Indexes
        builder
            .HasIndex(r => new { r.ProductId, r.Title })
            .HasDatabaseName($"UX_{nameof(Review)}_{nameof(ProductId)}_{nameof(Title)}")
            .IsUnique();
    }
}
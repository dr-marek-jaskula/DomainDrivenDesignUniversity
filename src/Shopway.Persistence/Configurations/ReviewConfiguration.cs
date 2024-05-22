using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Products;
using Shopway.Domain.Products.ValueObjects;
using Shopway.Domain.Users.ValueObjects;
using Shopway.Persistence.Converters.ValueObjects;
using Shopway.Persistence.Utilities;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;

namespace Shopway.Persistence.Configurations;

[GenerateEntityIdComparer(IdName = ReviewId.Name, IdNamespace = ReviewId.Namespace)]
[GenerateEntityIdConverter(IdName = ReviewId.Name, IdNamespace = ReviewId.Namespace)]
internal sealed class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(TableName.Review, SchemaName.Shopway);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion<ReviewIdConverter, ReviewIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));

        builder.Property(r => r.ProductId)
            .HasConversion<ProductIdConverter, ProductIdComparer>()
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
            .HasColumnType(ColumnType.TinyInt)
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
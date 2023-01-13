using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Domain.ValueObjects;
using Shopway.Domain.StronglyTypedIds;

namespace Shopway.Persistence.Configurations;

internal sealed class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(TableNames.Review, SchemaNames.Shopway);

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasConversion(id => id.Value, guid => ReviewId.New(guid))
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(r => r.Username)
            .HasConversion(x => x.Value, v => Username.Create(v).Value)
            .IsRequired(true)
            .HasMaxLength(100);

        builder.Property(s => s.Title)
            .HasConversion(x => x.Value, v => Title.Create(v).Value)
            .IsRequired(true)
            .HasMaxLength(128);

        builder.Property(s => s.Stars)
            .HasConversion(x => x.Value, v => Stars.Create(v).Value)
            .IsRequired(true)
            .HasColumnType("TINYINT");

        builder.Property(u => u.CreatedOn)
            .HasColumnType("datetimeoffset(2)");

        builder.Property(u => u.UpdatedOn)
            .HasColumnType("datetimeoffset(2)");

        builder.Property(r => r.Description)
            .HasConversion(x => x.Value, v => Description.Create(v).Value)
            .HasMaxLength(1000);

        builder.Property(r => r.ProductId)
            .HasConversion(id => id.Value, guid => ProductId.New(guid))
            .HasColumnType("UNIQUEIDENTIFIER")
            .IsRequired(true);
    }
}
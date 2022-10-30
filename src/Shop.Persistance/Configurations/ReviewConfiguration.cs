using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable(TableNames.Review);

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id)
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(r => r.Username)
            .IsRequired(true)
            .HasMaxLength(100);

        builder.Property(s => s.Stars)
            .IsRequired(true)
            .HasColumnType("TINYINT");

        builder.Property(u => u.CreatedOn)
            .HasDefaultValueSql("getutcdate()") //need to use HasDefaultValueSql with "getutcdate" because it need to be the sql command
            .HasColumnType("DATETIME2");

        builder.Property(u => u.UpdatedOn)
            .ValueGeneratedOnAddOrUpdate() //Generate the value when the update is made and when data is added
            .HasDefaultValueSql("getutcdate()") //need to use HasDefaultValueSql with "getutcdate" because it need to be the sql command
            .HasColumnType("DATETIME2");

        builder.Property(r => r.Description)
            .HasMaxLength(1000);
    }
}
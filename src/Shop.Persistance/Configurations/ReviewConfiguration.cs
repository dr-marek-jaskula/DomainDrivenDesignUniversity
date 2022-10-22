using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public sealed class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.ToTable("Review");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).UseIdentityColumn();

        builder.Property(r => r.UserName)
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

        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId);

        builder.HasOne(r => r.Employee)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.EmployeeId);
    }
}
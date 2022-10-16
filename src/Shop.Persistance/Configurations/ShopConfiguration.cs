using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public sealed class ShopEntityTypeConfiguration : IEntityTypeConfiguration<Shop>
{
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.ToTable("Shop");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Id).HasColumnType("TINYINT").UseIdentityColumn();

        builder.Property(s => s.Name)
            .IsRequired(true)
            .HasMaxLength(128);

        builder.Property(s => s.Description)
            .HasMaxLength(1000);

        builder.HasOne(s => s.Address)
            .WithOne(a => a.Shop)
            .HasForeignKey<Shop>(s => s.AddressId);

        builder.HasMany(s => s.Employees)
            .WithOne(e => e.Shop)
            .HasForeignKey(e => e.ShopId);

        builder.HasMany(s => s.Orders)
            .WithOne(o => o.Shop)
            .HasForeignKey(o => o.ShopId);
    }
}
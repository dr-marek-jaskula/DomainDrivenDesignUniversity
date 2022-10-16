using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;

namespace Shopway.Persistence.Configurations;

public class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Role");

        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasColumnType("TINYINT").UseIdentityColumn();

        builder.Property(r => r.Name)
            .HasDefaultValue("Customer")
            .HasColumnType("VARCHAR(13)")
            .HasComment("Customer, Employee, Manager, Administrator");

        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId);

        //Inserting static data (data that are not related to other)
        builder.HasData(
            new Role() { Id = Guid.NewGuid(), Name = "Customer" },
            new Role() { Id = Guid.NewGuid(), Name = "Employee" },
            new Role() { Id = Guid.NewGuid(), Name = "Manager" },
            new Role() { Id = Guid.NewGuid(), Name = "Administrator" }
            );
    }
}
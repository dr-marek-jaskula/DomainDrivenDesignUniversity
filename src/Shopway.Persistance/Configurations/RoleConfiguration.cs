using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Domain.ValueObjects;
using Shopway.Persistence.Constants;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Persistence.Converters;

namespace Shopway.Persistence.Configurations;

internal sealed class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(TableNames.Role);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion<StronglyTypedIdConverter<RoleId>>()
            .HasColumnType("UNIQUEIDENTIFIER");

        builder.Property(r => r.RoleName)
            .HasConversion(x => x.Value, v => RoleName.Create(v).Value)
            .HasDefaultValue(RoleName.Create(RoleName.AllowedRoles.First()).Value)
            .HasColumnType("VARCHAR(13)")
            .HasComment("Customer, Employee, Manager, Administrator");

        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId);

        //Inserting static data (data that are not related to other)
        builder.HasData(
            Role.Create(Guid.NewGuid(), RoleName.Create("Customer").Value),
            Role.Create(Guid.NewGuid(), RoleName.Create("Employee").Value),
            Role.Create(Guid.NewGuid(), RoleName.Create("Manager").Value),
            Role.Create(Guid.NewGuid(), RoleName.Create("Administrator").Value)
            );
    }
}
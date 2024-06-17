using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Users.Authorization;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(TableName.RolePermission, SchemaName.Master);

        builder.HasKey(x => new { x.RoleName, x.PermissionName });

        builder.Property(x => x.RoleName)
            .HasColumnType(ColumnType.VarChar(128));

        builder.Property(x => x.PermissionName)
            .HasColumnType(ColumnType.VarChar(128));

        //Insert static data
        builder.HasData
        (
            Create(RoleName.Administrator, PermissionName.Review_Read),
            Create(RoleName.Administrator, PermissionName.Review_Add),
            Create(RoleName.Administrator, PermissionName.Review_Update),
            Create(RoleName.Administrator, PermissionName.Review_Remove),
            Create(RoleName.Manager, PermissionName.Review_Read),
            Create(RoleName.Manager, PermissionName.Review_Add),
            Create(RoleName.Manager, PermissionName.Review_Update),
            Create(RoleName.Employee, PermissionName.Review_Read),
            Create(RoleName.Customer, PermissionName.Review_Read),
            Create(RoleName.Customer, PermissionName.Review_Add),
            Create(RoleName.Customer, PermissionName.Review_Update)
        );
    }

    private static RolePermission Create(RoleName role, PermissionName permission)
    {
        return new RolePermission(role.ToString(), permission.ToString());
    }
}

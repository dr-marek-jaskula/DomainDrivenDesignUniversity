using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Users.Enumerations;
using static Shopway.Persistence.Constants.Constants;
using Permission = Shopway.Domain.Users.Enumerations.Permission;

namespace Shopway.Persistence.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(TableName.RolePermission, SchemaName.Master);

        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        ////Insert static data
        builder.HasData
        (
            Create(Role.Administrator, Permission.Review_Read),
            Create(Role.Administrator, Permission.Review_Add),
            Create(Role.Administrator, Permission.Review_Update),
            Create(Role.Administrator, Permission.Review_Remove),
            Create(Role.Manager, Permission.Review_Read),
            Create(Role.Manager, Permission.Review_Add),
            Create(Role.Manager, Permission.Review_Update),
            Create(Role.Employee, Permission.Review_Read),
            Create(Role.Customer, Permission.Review_Read),
            Create(Role.Customer, Permission.Review_Add),
            Create(Role.Customer, Permission.Review_Update)
        );
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission(role.Id, permission.Id);
    }
}

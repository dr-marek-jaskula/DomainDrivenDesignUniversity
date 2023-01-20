using Shopway.Persistence.Constants;
using Shopway.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Shopway.Domain.Enumerations.Permission;

namespace Shopway.Persistence.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(TableNames.RolePermission, SchemaNames.Master);

        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        builder.HasData
        (
            Create(Role.Administrator, Permission.Read),
            Create(Role.Administrator, Permission.Create),
            Create(Role.Administrator, Permission.Update),
            Create(Role.Administrator, Permission.Delete),
            Create(Role.Manager, Permission.Read),
            Create(Role.Manager, Permission.Create),
            Create(Role.Manager, Permission.Update),
            Create(Role.Employee, Permission.Read),
            Create(Role.Employee, Permission.Create),
            Create(Role.Customer, Permission.Read)
        );
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = permission.Id
        };
    }
}

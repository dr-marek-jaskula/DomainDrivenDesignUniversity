using Shopway.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = Shopway.Domain.Enumerations.Permission;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(TableName.RolePermission, SchemaName.Master);

        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        //Insert static data
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
            Create(Role.Customer, Permission.Read),
            Create(Role.Customer, Permission.CRUD_Review)
        );
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission(role.Id, permission.Id);
    }
}

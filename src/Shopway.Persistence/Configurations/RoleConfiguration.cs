using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Users.Authorization;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(TableName.Role, SchemaName.Master);

        builder.HasKey(r => r.Name);

        builder.Property(r => r.Name)
            .HasColumnType(ColumnType.VarChar(128));

        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasMany(r => r.Users)
            .WithMany(u => u.Roles);

        var predefinedRoles = Role.GetPredefinedRoles();
        var rolesFromEnum = GetNamesOf<RoleName>();

        bool areEnumRolesEquivalentToPredefinedRoles = predefinedRoles
            .Select(x => x.Name)
            .SequenceEqual(rolesFromEnum);

        if (areEnumRolesEquivalentToPredefinedRoles is false)
        {
            throw new Exception($"{nameof(Role)} enum values are not equivalent to predefined {nameof(Role)}s");
        }

        //Insert static data
        builder.HasData(predefinedRoles);
    }
}

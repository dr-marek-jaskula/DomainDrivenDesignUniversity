using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Users.Enumerations;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(TableName.Role, SchemaName.Master);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnType(ColumnType.TinyInt);

        builder.Property(r => r.Name)
            .HasColumnType(ColumnType.VarChar(128));

        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasMany(r => r.Users)
            .WithMany(u => u.Roles);

        var rolesFromEnumeration = Role.GetNames();
        var rolesFromEnum = GetNamesOf<Domain.Enums.Role>();

        bool areEnumRolesEquivalentToEnumerationRoles =
            rolesFromEnumeration.SetEquals(rolesFromEnum);

        if (areEnumRolesEquivalentToEnumerationRoles is false)
        {
            throw new Exception($"{nameof(Role)} enum values are not equivalent to {nameof(Role)} enumeration values");
        }

        //Insert static data
        builder.HasData(Role.List);
    }
}

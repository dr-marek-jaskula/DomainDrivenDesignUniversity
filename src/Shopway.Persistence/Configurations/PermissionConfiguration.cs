using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Users.Enumerations;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableName.Permission, SchemaName.Master);

        builder.HasKey(p => p.Id);

        builder.Property(r => r.Id)
            .HasColumnType(ColumnType.TinyInt);

        builder.Property(r => r.Name)
            .HasColumnType(ColumnType.VarChar(128));

        builder.Ignore(r => r.HasAllProperties);
        builder.Ignore(r => r.RelatedAggregateRoot);
        builder.Ignore(r => r.RelatedEntity);
        builder.Ignore(r => r.Properties);
        builder.Ignore(r => r.RelatedEnum);
        builder.Ignore(r => r.Type);

        var permissionsFromEnumeration = Permission.GetNames();
        var permissionsFromEnum = GetNamesOf<Domain.Enums.Permission>();

        bool areEnumPermisionsEquivalentToEnumerationPermissions =
            permissionsFromEnumeration.SetEquals(permissionsFromEnum);

        if (areEnumPermisionsEquivalentToEnumerationPermissions is false)
        {
            throw new Exception($"{nameof(Permission)} enum values are not equivalent to {nameof(Permission)} enumeration values");
        }

        //Inserting static data (data that are not related to other)
        builder.HasData(Permission.List);
    }
}

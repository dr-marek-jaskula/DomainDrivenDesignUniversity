using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Users.Authorization;
using Shopway.Persistence.Converters.Enums;
using static Shopway.Domain.Common.Utilities.EnumUtilities;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableName.Permission, SchemaName.Master);

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasColumnType(ColumnType.TinyInt);

        builder.Property(p => p.Name)
            .HasColumnType(ColumnType.NVarChar(128))
            .IsRequired(true);

        builder.Property(p => p.RelatedAggregateRoot)
            .HasColumnType(ColumnType.NVarChar(128))
            .IsRequired(false);

        builder.Property(p => p.RelatedEntity)
            .HasColumnType(ColumnType.NVarChar(128))
            .IsRequired(false);

        builder.Property(p => p.Type)
            .HasConversion<PermissionTypeConverter>()
            .HasColumnType(ColumnType.VarChar(LongestOf<PermissionType>()))
            .IsRequired(true);

        builder.Property(p => p.Properties);

        var permissionsFromEnumeration = Permission.GetNames();
        var permissionsFromEnum = GetNamesOf<PermissionName>();

        bool areEnumPermisionsEquivalentToEnumerationPermissions =
            permissionsFromEnumeration.SetEquals(permissionsFromEnum);

        if (areEnumPermisionsEquivalentToEnumerationPermissions is false)
        {
            throw new Exception($"{nameof(Permission)} enum values are not equivalent to {nameof(Permission)} enumeration values");
        }

        //Inserting static data (data that are not related to other)
        builder.HasData(Permission.List);

        builder.HasIndex(x => x.Name)
            .IsUnique();
    }
}

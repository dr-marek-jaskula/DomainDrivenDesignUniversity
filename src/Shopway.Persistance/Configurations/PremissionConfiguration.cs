using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Shopway.Persistence.Exceptions;

namespace Shopway.Persistence.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNames.Permission);

        builder.HasKey(p => p.Id);


        var premissionsFromEnumeration = Permission
            .GetValues()
            .Select(p => p.Name)
            .ToHashSet();

        var premissionsFromEnum = Enum
            .GetValues<Domain.Enums.Permission>()
            .Select(p => p.ToString())
            .ToHashSet();

        bool areEnumPremisionsEquivalentToEnumerationPremissions =
            premissionsFromEnumeration.SetEquals(premissionsFromEnum);

        if (areEnumPremisionsEquivalentToEnumerationPremissions is false)
        {
            throw new ConfigurationException($"{nameof(Permission)} enum values are not equivalent to {nameof(Permission)} enumeration values");
        }

        //Inserting static data (data that are not related to other)
        builder.HasData(Permission.GetValues());
    }
}

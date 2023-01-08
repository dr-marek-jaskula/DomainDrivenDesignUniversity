using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(TableNames.Permission);

        builder.HasKey(p => p.Id);

        //Inserting static data (data that are not related to other)
        builder.HasData(Role.GetValues());
    }
}

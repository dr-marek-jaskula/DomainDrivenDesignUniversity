using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Converters.EntityIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shopway.Persistence.Configurations;

internal sealed class RoleUserConfiguration : IEntityTypeConfiguration<RoleUser>
{
    public void Configure(EntityTypeBuilder<RoleUser> builder)
    {
        builder.ToTable(TableNames.RoleUser, SchemaNames.Master);

        builder.HasKey(x => new { x.RoleId, x.UserId });

        builder.Property(x => x.UserId)
            .HasConversion<UserIdConverter, UserIdComparer>();
    }
}

using Shopway.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Converters.EntityIds;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Configurations;

internal sealed class RoleUserConfiguration : IEntityTypeConfiguration<RoleUser>
{
    public void Configure(EntityTypeBuilder<RoleUser> builder)
    {
        builder.ToTable(TableName.RoleUser, SchemaName.Master);

        builder.HasKey(x => new { x.RoleId, x.UserId });

        builder.Property(x => x.UserId)
            .HasConversion<UserIdConverter, UserIdComparer>();
    }
}

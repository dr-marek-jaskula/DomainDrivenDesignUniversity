using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Users.Authorization;
using Shopway.Persistence.Converters.EntityIds;
using static Shopway.Persistence.Constants.Constants;
using static Shopway.Persistence.Constants.Constants.Number;

namespace Shopway.Persistence.Configurations;

internal sealed class RoleUserConfiguration : IEntityTypeConfiguration<RoleUser>
{
    public void Configure(EntityTypeBuilder<RoleUser> builder)
    {
        builder.ToTable(TableName.RoleUser, SchemaName.Master);

        builder.HasKey(x => new { x.RoleName, x.UserId });

        builder.Property(x => x.RoleName)
            .HasColumnType(ColumnType.VarChar(128));

        builder.Property(x => x.UserId)
            .HasConversion<UserIdConverter, UserIdComparer>()
            .HasColumnType(ColumnType.Char(UlidCharLenght));
    }
}

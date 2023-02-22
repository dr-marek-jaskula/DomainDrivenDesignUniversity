using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Constants;
using Shopway.Domain.Enumerations;

namespace Shopway.Persistence.Configurations;

internal sealed class RoleEntityTypeConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(TableNames.Role, SchemaNames.Master);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnType(ColumnTypes.TinyInt);

        builder.Property(r => r.Name)
            .HasColumnType(ColumnTypes.VarChar(128));

        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();

        builder.HasMany(r => r.Users)
            .WithMany(u => u.Roles);

        //Insert static data
        builder.HasData(Role.List);
    }
}
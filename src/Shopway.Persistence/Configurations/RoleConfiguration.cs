﻿using Shopway.Domain.Enumerations;
using Microsoft.EntityFrameworkCore;
using static Shopway.Persistence.Constants.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        //Insert static data
        builder.HasData(Role.List);
    }
}
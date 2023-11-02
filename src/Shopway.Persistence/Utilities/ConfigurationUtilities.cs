using Shopway.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Utilities;

public static class ConfigurationUtilities
{
    public static EntityTypeBuilder<TEntity> ConfigureAuditableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntity, IAuditable
    {
        builder.Property(o => o.CreatedOn)
            .HasColumnType(ColumnType.DateTimeOffset(2));

        builder.Property(o => o.UpdatedOn)
            .HasColumnType(ColumnType.DateTimeOffset(2))
            .IsRequired(false);

        builder.Property(o => o.CreatedBy)
            .HasColumnType(ColumnType.VarChar(30));

        builder.Property(o => o.UpdatedBy)
            .HasColumnType(ColumnType.VarChar(30))
            .IsRequired(false);

        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigureSoftDeletableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
    where TEntity : class, IEntity, ISoftDeletable
    {
        builder.Property(o => o.SoftDeleted)
            .HasColumnType(ColumnType.Bit)
            .HasDefaultValue(false);

        builder.Property(o => o.SoftDeletedOn)
            .HasColumnType(ColumnType.DateTimeOffset(2))
            .IsRequired(false);

        return builder;
    }

    public static EntityTypeBuilder<TEntity> ConfigureDiscriminator<TEntity>(this EntityTypeBuilder<TEntity> builder, string discriminatorName = "Discriminator")
        where TEntity : class, IEntity
    {
        builder.HasDiscriminator<string>(discriminatorName);

        builder.Property(discriminatorName)
            .HasMaxLength(100)
            .HasColumnName(discriminatorName);

        return builder;
    }
}
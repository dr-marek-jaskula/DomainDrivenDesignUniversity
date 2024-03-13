using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using static Shopway.Persistence.Constants.Constants;

namespace Shopway.Persistence.Utilities;

public static class ConfigurationUtilities
{
    /// <summary>
    /// Configure an entity to be auditable. The "UpdatedOn" column will be used for concurrency by default.
    /// So when the two simultaneous requests would aim to change the auditable entity, the first one would succeed, while the second one would fail, because the "UpdatedOn" properties would differ.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="builder"></param>
    /// <param name="useUpdatedOnAsConcurrencyToken">If true, the UpdatedOn column will be used as a ConcurrencyToken and its precision will be increased up to nanoseconds</param>
    /// <returns></returns>
    public static EntityTypeBuilder<TEntity> ConfigureAuditableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder, bool useUpdatedOnAsConcurrencyToken = true)
        where TEntity : class, IEntity, IAuditable
    {
        builder.Property(o => o.CreatedBy)
            .HasColumnType(ColumnType.VarChar(30));

        builder.Property(o => o.CreatedOn)
            .HasColumnType(ColumnType.DateTimeOffset(2));

        builder.Property(o => o.UpdatedBy)
            .HasColumnType(ColumnType.VarChar(30))
            .IsRequired(false);

        if (useUpdatedOnAsConcurrencyToken)
        {
            builder.Property(o => o.UpdatedOn)
                .HasColumnType(ColumnType.DateTimeOffset(7))
                .IsConcurrencyToken(true)
                .IsRequired(false);

            return builder;
        }

        builder.Property(o => o.UpdatedOn)
            .HasColumnType(ColumnType.DateTimeOffset(2))
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
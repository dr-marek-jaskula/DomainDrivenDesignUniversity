using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopway.Domain.Abstractions;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence.Utilities;

public static class ConfigureUtilities
{
    public static EntityTypeBuilder<TEntity> ConfigureAuditableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IEntity, IAuditable
    {
        builder.Property(o => o.CreatedOn)
            .HasColumnType(ColumnTypes.DateTimeOffset(2));

        builder.Property(o => o.UpdatedOn)
            .HasColumnType(ColumnTypes.DateTimeOffset(2))
            .IsRequired(false);

        builder.Property(o => o.CreatedBy)
            .HasColumnType(ColumnTypes.VarChar(30));

        builder.Property(o => o.UpdatedBy)
            .HasColumnType(ColumnTypes.VarChar(30))
            .IsRequired(false);

        return builder;
    }
}
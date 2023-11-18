using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Shopway.Persistence.Abstractions;

//When inherit from this class, override the ConfigureNotMigratedEntity and implement there the standard configuration
//Then ApplyConfiguration explicitly in OnModelCreating
public abstract class ExcludeFromMigrationsEntityTypeConfigurationBase<TEntity>(bool excludeFromMigrations) : IEntityTypeConfiguration<TEntity>
    where TEntity : class
{
    protected readonly bool _excludeFromMigrations = excludeFromMigrations;

    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        ExcludeFromMigrations(builder);
        ConfigureNotMigratedEntity(builder);
    }

    protected abstract void ConfigureNotMigratedEntity(EntityTypeBuilder<TEntity> builder);

    private void ExcludeFromMigrations(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .Metadata
            .SetIsTableExcludedFromMigrations(_excludeFromMigrations);
    }
}
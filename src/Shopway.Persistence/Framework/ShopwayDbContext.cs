using Microsoft.EntityFrameworkCore;

namespace Shopway.Persistence.Framework;

public sealed class ShopwayDbContext : DbContext
{
    public ShopwayDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
    }
}

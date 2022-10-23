using Microsoft.EntityFrameworkCore;
using Shopway.Domain.Entities;
using Shopway.Persistence.Constants;

namespace Shopway.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

        //Here is configure all many to many relationships
        
        //Product_Tag
        builder.Entity<Product>(entityBuilder =>
        {
            entityBuilder
                .HasMany(p => p.Tags)
                .WithMany(t => t.Products)
                .UsingEntity(j => j.ToTable(TableNames.ProductTag));
        });
    }
}

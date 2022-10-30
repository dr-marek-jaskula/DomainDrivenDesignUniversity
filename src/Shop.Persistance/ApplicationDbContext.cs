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
    }
}

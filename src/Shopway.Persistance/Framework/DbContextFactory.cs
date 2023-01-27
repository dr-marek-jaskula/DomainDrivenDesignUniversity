using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Shopway.Persistence.Framework;

public sealed class ShopwayDbContextFactory : IDesignTimeDbContextFactory<ShopwayDbContext>
{
    public ShopwayDbContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("connectionString.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<ShopwayDbContext>();

        if (args is not null && args.Contains("TestConnection") is true)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("TestConnection"));
        }
        else
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        }

        return new ShopwayDbContext(optionsBuilder.Options);
    }
}
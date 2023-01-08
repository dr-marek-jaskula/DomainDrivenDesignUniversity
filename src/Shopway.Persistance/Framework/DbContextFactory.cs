using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Shopway.Persistence.Framework;

public sealed class MyDbContextFactory : IDesignTimeDbContextFactory<ShopwayDbContext>
{
    public ShopwayDbContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("connectionString.json").Build();

        var optionsBuilder = new DbContextOptionsBuilder<ShopwayDbContext>();
        optionsBuilder.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);

        return new ShopwayDbContext(optionsBuilder.Options);
    }
}
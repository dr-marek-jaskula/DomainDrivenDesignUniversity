using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using static Shopway.Persistence.Constants.Constants.Connection;

namespace Shopway.Persistence.Framework;

public sealed class ShopwayDbContextFactory : IDesignTimeDbContextFactory<ShopwayDbContext>
{
    public ShopwayDbContext CreateDbContext(string[]? args = null)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile(ConnectionStringJsonFile).Build();

        var optionsBuilder = new DbContextOptionsBuilder<ShopwayDbContext>();

        if (args is not null && args.Contains(TestConnection) is true)
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(TestConnection));
        }
        else if (args is not null && args.Length is 1)
        {
            optionsBuilder.UseSqlServer(args.Single());
        }
        else
        {
            optionsBuilder.UseSqlServer(configuration.GetConnectionString(DefaultConnection));
        }

        return new ShopwayDbContext(optionsBuilder.Options);
    }
}

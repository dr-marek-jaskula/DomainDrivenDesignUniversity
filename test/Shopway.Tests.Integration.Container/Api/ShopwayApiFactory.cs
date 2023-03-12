using Quartz;
using Testcontainers.MsSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shopway.Persistence.Framework;
using Shopway.Tests.Integration.Configurations;
using Shopway.Infrastructure.Options;
using static Shopway.Tests.Integration.Constants.IntegrationTestsConstants;

namespace Shopway.Tests.Integration;

public sealed class ShopwayApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    public string ContainerConnectionString { get; private set; } = string.Empty;

    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .WithName(SqlServerContainerName)
        .Build();

    public ShopwayApiFactory()
    {
    } 

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            ContainerConnectionString = _msSqlContainer.GetConnectionString();

            services.RemoveAll(typeof(IHostedService));
            services.RemoveAll(typeof(IJob));
            services.RemoveAll(typeof(ShopwayDbContext));

            services.Configure<DatabaseOptions>(options =>
            {
                options.ConnectionString = ContainerConnectionString;
            });

            services.AddSingleton(x => new IntegrationTestsUrlOptions()
            {
                ShopwayApiUrl = "https://localhost:7236/api/"
            });

            services.AddSingleton(x => new ApiKeyTestOptions()
            {
                PRODUCT_GET = "d3f72374-ef67-42cb-b25b-fbfee58b1054",
                PRODUCT_UPDATE = "ae5bd500-6d11-4f67-950f-85d87b1d81c4",
                PRODUCT_REMOVE = "36777477-d70c-4a9a-b5bd-a1eb286fa16b",
                PRODUCT_CREATE = "51b4c4e8-d246-4dcf-b7c7-05811a9123c0"
            });

            services.RegisterDatabaseContext(true);
        });
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    //This method from the interface IAsyncLifetime will hide the one in the "WebApplicationFactory<IApiMarker>"
    //This in intentional hiding
    public new async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }
}
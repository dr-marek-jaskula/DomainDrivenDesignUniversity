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
using Shopway.Infrastructure.Authentication.ApiKeyAuthentication;
using Shopway.Tests.Integration.Container.Persistance;
using Shopway.Infrastructure.Authentication.PermissionAuthentication;
using Shopway.Persistence.Abstractions;

namespace Shopway.Tests.Integration;

public sealed class ShopwayApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    public string ContainerConnectionString { get; private set; } = string.Empty;

    private readonly MsSqlContainer _msSqlContainer = new MsSqlBuilder()
        .Build();

    public ShopwayApiFactory()
    {
    } 

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        //Set container connection string
        ContainerConnectionString = _msSqlContainer.GetConnectionString();

        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
        });

        builder.ConfigureTestServices(services =>
        {
            //Remove background workers
            services.RemoveAll(typeof(IHostedService));
            services.RemoveAll(typeof(IJob));

            //Register options
            services.AddSingleton(x => new IntegrationTestsUrlOptions()
            {
                ShopwayApiUrl = "https://localhost:7236/api/"
            });

            //Mock api key authentication
            services.RemoveAll(typeof(IApiKeyService));
            services.AddScoped<IApiKeyService, TestApiKeyService>();

            //Mock jwt authentication
            services.RemoveAll(typeof(IPermissionService));
            services.AddScoped<IPermissionService, TestPermissionService>();

            //Mock user context
            services.RemoveAll(typeof(IUserContextService));
            services.AddScoped<IUserContextService, TestUserContextService>();

            //Re-register database context to use the connection to the database in the container
            services.Configure<DatabaseOptions>(options =>
            {
                options.ConnectionString = ContainerConnectionString;
            });

            services.RemoveAll(typeof(ShopwayDbContext));
            services.RegisterDatabaseContext(true);
        });
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();
    }

    //This method from the interface IAsyncLifetime will hide the one in the "WebApplicationFactory<IApiMarker>"
    public new async Task DisposeAsync()
    {
        await _msSqlContainer.DisposeAsync();
    }
}
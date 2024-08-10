using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Shopway.App.wwwroot;
using Shopway.Application.Abstractions;
using Shopway.Domain.Users.Authorization;
using Shopway.Infrastructure.Options;
using Shopway.Persistence.Framework;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;
using Shopway.Tests.Integration.Configurations;
using Shopway.Tests.Integration.Container.Api;
using Testcontainers.MsSql;

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
            //for e2e testing remember to use:
            //services.AddQuartz(options =>
            //{
            //    var ulid = Ulid.NewUlid();
            //    options.SchedulerId = $"job-id-{ulid}";
            //    options.SchedulerName = $"job-name-{ulid}";
            //});

            //Register options
            services.AddSingleton(x => new IntegrationTestsUrlOptions()
            {
                ShopwayApiUrl = "https://localhost:7236/api/"
            });

            //Mock api key authentication
            services.RemoveAll(typeof(IApiKeyService<ApiKey>));
            services.AddScoped<IApiKeyService<ApiKey>, TestApiKeyService<ApiKey>>();

            //Mock jwt authentication
            services.RemoveAll(typeof(IUserAuthorizationService<PermissionName, RoleName>));
            services.AddScoped<IUserAuthorizationService<PermissionName, RoleName>, TestUserAuthorizationService>();

            //Mock user context
            services.RemoveAll(typeof(IUserContextService));
            services.AddScoped<IUserContextService, TestUserContextService>();

            //Add AuthorizationHandlers that will succeed all requirements
            services.RemoveAll<IAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler, TestPermissionRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, TestRoleRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, TestApiKeyRequirementHandler>();

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

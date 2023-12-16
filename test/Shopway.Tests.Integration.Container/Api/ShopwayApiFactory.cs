﻿using Quartz;
using Shopway.App.wwwroot;
using Testcontainers.MsSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.TestHost;
using Shopway.Persistence.Framework;
using Shopway.Infrastructure.Options;
using Microsoft.AspNetCore.Mvc.Testing;
using Shopway.Application.Abstractions;
using Shopway.Tests.Integration.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Tests.Integration.Container.Persistance;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shopway.Presentation.Authentication.ApiKeyAuthentication;
using Shopway.Presentation.Authentication.RolePermissionAuthentication;

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
            services.RemoveAll(typeof(IUserAuthorizationService));
            services.AddScoped<IUserAuthorizationService, TestUserAuthorizationService>();

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
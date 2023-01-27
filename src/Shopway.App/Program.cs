using Microsoft.EntityFrameworkCore;
using Serilog.Events;
using Serilog;
using Shopway.Presentation.Exceptions;
using Shopway.Persistence.Framework;

Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console() 
            .CreateBootstrapLogger();

try
{
    Log.Information("Staring the web host");
    
    //Initial configuration

    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = Directory.GetCurrentDirectory()
    });

    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());

    //Configure Services

    builder.Services
        .RegisterControllers()
        .RegisterOptions()
        .RegisterFluentValidation()
        .RegisterMediator()
        .AddMemoryCache()
        .RegisterDatabaseContext(builder.Environment.IsDevelopment())
        .RegisterBackgroundServices()
        .RegisterMiddlewares()
        .RegisterAuthentication()
        .RegisterServices()
        .RegisterServiceDecorators()
        .RegisterRepositories()
        .RegisterHealthCheck(builder.Configuration)
        .AddSwaggerGen();

    //Build the application

    WebApplication app = builder.Build();

    //Configure HTTP request pipeline

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
    });

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app
        .UseHealthChecks()
        .UseMiddlewares()
        .UseHttpsRedirection();

    #region Apply Migrations
    
    var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var applyMigrationsScope = serviceScopeFactory.CreateScope())
    {
        var dbContext = applyMigrationsScope.ServiceProvider.GetService<ShopwayDbContext>();

        if (dbContext is null)
            throw new UnavailableException("Database is not available");

        var pendingMigrations = dbContext.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
            dbContext.Database.Migrate();
    }

    #endregion Apply Migrations

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;

sealed partial class Program { }
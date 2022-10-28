using Microsoft.EntityFrameworkCore;
using Shopway.Persistence;
using Shopway.App.Registration;
using Serilog.Events;
using Serilog;
using Shopway.Presentation.Exceptions;

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

    builder.Services.AddControllers()
        .AddApplicationPart(Shopway.Presentation.AssemblyReference.Assembly);

    builder.Services.RegisterOptions();

    builder.Services.RegisterFluentValidation();

    builder.Services.RegisterMediator();

    builder.Services.RegisterDatabaseContext(builder.Environment.IsDevelopment());

    builder.Services.RegisterServices();

    builder.Services.RegisterServiceDecorators();

    //TODO something like quartz????????? (logging here and other stuff) QUARTZ is for background jobs?

    builder.Services.RegisterMiddlewares();

    builder.Services.AddSwaggerGen();

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

    app.UseMiddlewares();

    app.UseHttpsRedirection();

    #region Apply Migrations
    
    var serviceScopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var applyMigrationsScope = serviceScopeFactory.CreateScope())
    {
        var dbContext = applyMigrationsScope.ServiceProvider.GetService<ApplicationDbContext>();

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
using Serilog;
using static Microsoft.Extensions.DependencyInjection.LoggerUtilities;

Log.Logger = CreateSerilogLogger();

try
{
    Log.Information("Staring the web host");

    //Initial configuration

    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = Directory.GetCurrentDirectory()
    });

    builder.ConfigureSerilog();

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
        .RegisterHealthChecks()
        .RegisterVersioning()
        .RegisterOpenApi();

    //Build the application

    WebApplication webApplication = builder.Build();

    //Configure HTTP request pipeline

    webApplication
        .ConfigureOpenApi()
        .ConfigureSerilogRequestLogging()
        .UseStaticFiles()
        .UseHealthChecks()
        .UseMiddlewares()
        .UseHttpsRedirection()
        .ApplyMigrations()
        .UseAuthorization();

    webApplication.MapControllers();

    //Run the application
    webApplication.Run();
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
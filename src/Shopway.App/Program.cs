using Microsoft.Extensions.DependencyInjection;
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
        .RegisterAppOptions()
        .RegisterApplicationLayer()
        .RegisterPersistenceLayer(builder.Environment, builder.Logging)
        .RegisterInfrastructureLayer()
        .RegisterPresentationLayer();

    //Build the application

    WebApplication webApplication = builder.Build();

    //Configure HTTP request pipeline

    webApplication
        .UseHttpsRedirection()
        .UseApplicationLayer()
        .UsePresentationLayer(builder.Environment)
        .UsePersistenceLayer();

    webApplication.MapControllers();

    //Run the application
    webApplication.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.Information("Ending the web host");
    Log.CloseAndFlush();
}

return 0;

sealed partial class Program { }
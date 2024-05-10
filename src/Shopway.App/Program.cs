try
{
    Console.WriteLine("Staring the web host");

    //Initial configuration

    var builder = WebApplication.CreateBuilder(new WebApplicationOptions
    {
        Args = args,
        ContentRootPath = Directory.GetCurrentDirectory()
    });

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
    Console.Error.WriteLine($"Host terminated unexpectedly. Exception: {exception.Message}");
    return 1;
}
finally
{
    Console.WriteLine("Ending the web host");
}

return 0;

sealed partial class Program { }
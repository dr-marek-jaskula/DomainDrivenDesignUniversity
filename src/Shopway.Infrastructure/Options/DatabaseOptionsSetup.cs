using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace Shopway.Infrastructure.Options;

public sealed class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private readonly string _configurationSectionName = "DatabaseOptions";
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;

    public DatabaseOptionsSetup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _environment = environment;
    }

    public void Configure(DatabaseOptions options)
    {
        if (_environment.IsProduction() is true)
        {
            options.ConnectionString = _configuration
                .GetConnectionString("DefaultConnection");
        }
        else if (_environment.IsDevelopment() is true)
        {
            options.ConnectionString = _configuration
                .GetConnectionString("TestConnection");
        }

        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}

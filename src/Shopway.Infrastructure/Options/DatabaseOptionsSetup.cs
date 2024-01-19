using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

internal sealed class DatabaseOptionsSetup(IConfiguration configuration, IWebHostEnvironment environment) : IConfigureOptions<DatabaseOptions>
{
    private const string _configurationSectionName = "DatabaseOptions";
    private const string _defaultConnection = "DefaultConnection";
    private const string _testConnection = "TestConnection";
    private readonly IConfiguration _configuration = configuration;
    private readonly IWebHostEnvironment _environment = environment;

    public void Configure(DatabaseOptions options)
    {
        if (_environment.IsProduction() is true)
        {
            options.ConnectionString = _configuration
                .GetConnectionString(_defaultConnection);
        }
        else if (_environment.IsDevelopment() is true)
        {
            options.ConnectionString = _configuration
                .GetConnectionString(_testConnection);
        }

        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}

using Microsoft.Extensions.Options;

namespace Shopway.App.Options;

public sealed class DatabaseOptionsSetup : IConfigureOptions<DatabaseOptions>
{
    private readonly string _configurationSectionName = "DatabaseOptions";
    private readonly IConfiguration _configuration;

    public DatabaseOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DatabaseOptions options)
    {
        options.ConnectionString = _configuration
            .GetConnectionString("DefaultConnection");
        
        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}

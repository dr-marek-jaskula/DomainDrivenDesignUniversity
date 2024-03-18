using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Shopway.Infrastructure.Options;

internal sealed class MailSenderOptionsSetup(IConfiguration configuration) : IConfigureOptions<MailSenderOptions>
{
    private const string _configurationSectionName = "MailSenderOptions";
    private readonly IConfiguration _configuration = configuration;

    public void Configure(MailSenderOptions options)
    {
        _configuration
            .GetSection(_configurationSectionName)
            .Bind(options);
    }
}

namespace Shopway.Infrastructure.Options;

public sealed class AuthenticationOptions
{
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int DaysToExpire { get; set; }
}

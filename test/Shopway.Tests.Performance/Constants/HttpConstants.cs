namespace Shopway.Tests.Performance.Constants;

public static class HttpConstants
{
    public static readonly HttpClient HttpClient = new();
    public const string GET = nameof(GET);
    public const string ApiKey = "x-api-key";
}
namespace Shopway.Tests.Performance.Constants;

public static partial class Constants
{
    public static class Http
    {
        public static readonly HttpClient Client = new();
        public const string GET = nameof(GET);
        public const string ApiKey = "x-api-key";
    }
}
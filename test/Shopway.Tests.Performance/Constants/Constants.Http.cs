namespace Shopway.Tests.Performance.Constants;

public static partial class Constants
{
    public static class Http
    {
        public const string GET = nameof(GET);
        public const string POST = nameof(POST);
        public const string PUT = nameof(PUT);
        public const string PATCH = nameof(PATCH);
        public const string DELETE = nameof(DELETE);
        public const string ApiKey = "x-api-key";
        public const string JsonMediaType = "application/json";
        public const string ContentType = "Content-Type";
        public const string Authorization = nameof(Authorization);
    }
}
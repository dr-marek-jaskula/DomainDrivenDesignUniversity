using NBomber.Http.CSharp;
using Newtonsoft.Json;
using Shopway.Application.Features.Users.Commands;
using static Shopway.Tests.Performance.Constants.Constants.Http;

namespace Shopway.Tests.Performance.Utilities;

public static class HttpClientUtilities
{
    public static async Task<HttpClient> WithBearerToken(this HttpClient client, string authorizationEndpoint, HttpContent httpContent)
    {
        var bearerToken = await GetBearerToken(client, authorizationEndpoint, httpContent);
        client.DefaultRequestHeaders.Add(Authorization, BearerAuthorization(bearerToken));
        return client;
    }

    public static HttpClient WithApiKey(this HttpClient client, string apiKey)
    {
        client.DefaultRequestHeaders.Remove(ApiKey);
        client.DefaultRequestHeaders.Add(ApiKey, apiKey);
        return client;
    }

    private static async Task<string> GetBearerToken(HttpClient client, string authorizationEndpoint, HttpContent httpContent)
    {
        if (authorizationEndpoint is null)
        {
            return string.Empty;
        }

        var request = Http.CreateRequest(POST, authorizationEndpoint)
            .WithBody(httpContent);

        var response = await client.SendAsync(request);

        var contentAsString = await response.Content.ReadAsStringAsync();
        var accessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(contentAsString)!;

        return accessTokenResponse.AccessToken;
    }

    private static string BearerAuthorization(string bearerToken)
    {
        return $"bearer {bearerToken}";
    }
}
# Tests.Integration Layer :heavy_check_mark:

This layer contains the integrations tests. 

Here we use the proper approach of using the WebApplicationFactory.

Moreover, we use TestContainers library, to make all integrations test in sql database inside the container, so the additional database cleaning is not required (like in Shopway.Tests.Integration)

## Mock ApiKey authentication

The preferred way is to use TestApiKeyService that always returns true:

```csharp
public sealed class TestApiKeyService : IApiKeyService
{
    public bool IsProvidedApiKeyEqualToRequiredApiKey(RequiredApiKey requiredApiKey, string? apiKeyFromHeader)
    {
        return true;
    }
}
```

and substitute IApiKeyService registration in ShopwayApiFactory

```csharp
services.RemoveAll(typeof(IApiKeyService));
services.AddScoped<IApiKeyService, TestApiKeyService>();
```

Finally, we need to add api key header to requests (at ControllerTestBase):

```csharp
/// <summary>
/// This method add api key header with dummy value for every request. IApiKeyService should be mocked to return true for each validation. 
/// </summary>
/// <param name="client">RestClient that will be used for tests</param>
private static void EnsureApiKeyAuthenticationForMockedIApiKeyService(RestClient client)
{
    client.AddDefaultHeader(ApiKeyHeader, nameof(ApiKeyHeader));
}
```

## Manual ApiKey authentication

At ShopwatApiFactory:

```csharp
services.AddSingleton(x => new ApiKeyTestOptions()
{
    PRODUCT_GET = "d3f72374-ef67-42cb-b25b-fbfee58b1054",
    PRODUCT_UPDATE = "ae5bd500-6d11-4f67-950f-85d87b1d81c4",
    PRODUCT_REMOVE = "36777477-d70c-4a9a-b5bd-a1eb286fa16b",
    PRODUCT_CREATE = "51b4c4e8-d246-4dcf-b7c7-05811a9123c0"
});
```

Then, at ControllerTestBase we add

```csharp
apiKeys = apiFactory.Services.GetRequiredService<ApiKeyTestOptions>();
```

where 

```csharp
protected readonly ApiKeyTestOptions apiKeys;
```

Finally, then we will need to add api keys headers like that:

```csharp
request.AddApiKeyAuthentication(apiKeys.PRODUCT_GET);
```

## Mock jwt authentication

The preferred way is to use TestPermissionService that always confirms the authentication:

```csharp
public sealed class TestPermissionService : IPermissionService
{
    private static readonly Guid _testUserGuid = Guid.Parse("2df39cb3-8645-4462-8d3f-06b6f1547b9f");

    public Result<UserId> GetUserId(AuthorizationHandlerContext context)
    {
        return UserId.Create(_testUserGuid);
    }

    public Task<bool> HasPermissionAsync(UserId userId, string permission)
    {
        return Task.FromResult(true);
    }
}
```

and substitute ITestPermissionService registration in ShopwayApiFactory

```csharp
services.RemoveAll(typeof(IPermissionService));
services.AddScoped<IPermissionService, TestPermissionService>();
```

## Manual jwt authentication

See "Shopway.Tests.Integration" in Abstraction/ControllerTestBase where we ensure that the test user is created with all privileges and the we log him for test purposes.

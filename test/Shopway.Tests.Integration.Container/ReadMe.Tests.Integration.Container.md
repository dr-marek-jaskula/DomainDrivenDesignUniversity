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

Finally, we need to add api key header to requests:

```csharp

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

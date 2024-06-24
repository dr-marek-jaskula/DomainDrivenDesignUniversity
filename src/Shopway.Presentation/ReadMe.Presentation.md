# Presentation Layer :door: 

This layer is the part where interaction with external user or other systems happens. 

Therefore, we define here:

- Controllers
- Request Exceptions
- ProblemDetails definition
- Authorization components
- OpenApi configuration

## Authorization

There are two authorization approaches implemented in this project:
1. Permission approach (examine if user has required permission). See ProductsController.Reviews.cs.
2. ApiKey approach (examine if request contains required api key in the "X-Api-Key" header for given endpoint). See ProductController.

Note: Remember to set the ClockSkew to 5s (or max 30s)

```csharp
options.TokenValidationParameters.ClockSkew = TimeSpan.FromSeconds(_authenticationOptions.ClockSkew); 
```

## TwoFactorAuthorization With Email

At first use the ```/users/login/two-factor/first-step``` endpoint to trigger the email sender. Get the code from email (papercut) and use the second endpoint 
```/users/login/two-factor/second-step```. TwoFactorToken is not stored in database - its hash is stored, both with date of its creation. 

If the invalid token is sent, then the token hash in the database is set to null. 

Papercut email service is used to demonstrate the two step verification. To get the email with code, use docker compose or run
```
docker run --name=papercut -p 25:25 -p 37408:37408 jijiechen/papercut:latest -d
```

Single step authorization is still valid, to show possible options.

## TwoFactorAuthorization With TOPT (Microsoft Authenticator)

At first use the ```/users/configure/two-factor/topt``` endpoint to configure topt secret. This will return an encoded imaged of `QR code`. To decode it use some online tool `Base64 to PNG`, for instance `https://base64.guru/converter/decode/image/png`. Then, use Microsoft Authenticator to scan the QR Code. Secondly, use `/users/login/two-factor/topt` endpoint and provide the current code from
Microsoft Authenticator. 

If You want to change the secret, just use configure endpoint again and again scan the `QR code`.

## Login by Google

1. Go to GoogleCloud **APIs & Services** (create google project at first if needed)
2. Go to Credentials and click "Create Credentials" and choose **OAuth client ID** and then **Web application**
3. Set project and uri.
4. Create credentials and safety store clientId and secret (not directly in appsettings!)
1. 
## Enum to string conversion

Due to the 

```csharp
.AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new RequiredPropertiesCamelCaseContractResolver();
    options.SerializerSettings.Formatting = Indented;
    options.SerializerSettings.Converters.Add(new StringEnumConverter());
    options.SerializerSettings.ReferenceLoopHandling = Ignore;
});
```

in the .App, we can convert strings to enums in the requests behind the scenes.

## EntityId Converter

Due to the EntityIdConverter, the conversion from string in route to entity id object is done behind the scenes.

## Routing

It is important to keep to the plural: use "products" not "product" for the route. 
Due to the fact that base ApiController has attribute 
```csharp
[Route("api/[controller]")]
```
we should name our controllers **ProductsController** and not ProductController.

## FastEndpoints

FastEndpoints are the go to approach for organizing presentation layer with endpoints pattern. They are add a significant performance boosts and provide multiple
feature from out of the box. One of the advantage of FastEndpoitns over MinimalApi is the possibility to overwrite RequestDeserializer, which is not possible
for MinialApi:

```csharp
options.Serializer.RequestDeserializer = async (request, dto, jCtx, cancellationToken) =>
{
    using var reader = new StreamReader(request.Body);
    return JsonConvert.DeserializeObject(await reader.ReadToEndAsync(), dto, _jsonSerializerSettings);
};
```
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
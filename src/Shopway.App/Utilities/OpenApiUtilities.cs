using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shopway.App.Utilities;

public static class OpenApiUtilities
{
    private const string JwtAuthorizationHeader = "Authorization";
    private const string ApiKeyAuthorization = "x-api-key";
    private const string JwtAuthorizationName = "oauth2";
    private const string Jwt = nameof(Jwt);
    private const string ApiKeyScheme = nameof(ApiKeyScheme);
    private const string Bearer = nameof(Bearer);
    private const string ApiKey = nameof(ApiKey);

    /// <summary>
    /// Used to add to OpenApi a new custom header
    /// </summary>
    /// <param name="options"></param>
    /// <param name="headerName">Name of the header</param>
    /// <param name="headerDescription">Header description</param>
    /// <param name="isHeaderRequiredInOpenApi">True if header is required, false otherwise. Default false</param>
    public static void AddCustomHeader(this SwaggerGenOptions options, string headerName, string headerDescription, bool isHeaderRequiredInOpenApi = false)
    {
        options.OperationFilter<AddHeaderOperationFilter>(headerName, headerDescription, isHeaderRequiredInOpenApi);
    }

    public static void AddJwtAuthrization(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(JwtAuthorizationName, new OpenApiSecurityScheme
        {
            Description = $"Bearer authorization. Input: \"{{token}}\"",
            In = ParameterLocation.Header,
            Name = JwtAuthorizationHeader,
            Type = SecuritySchemeType.Http,
            BearerFormat = Jwt,
            Scheme = Bearer
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = Bearer
                        },
                        In = ParameterLocation.Header
                    },
                    Array.Empty<string>()
                }
            });
    }

    public static void AddApiKeyAuthrization(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(ApiKey, new OpenApiSecurityScheme
        {
            Description = $"ApiKey authorization. Input: \"{{apikey}}\"",
            In = ParameterLocation.Header,
            Name = ApiKeyAuthorization,
            Type = SecuritySchemeType.ApiKey,
            Scheme = ApiKeyScheme
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = ApiKey
                    },
                    In = ParameterLocation.Header
                },
                Array.Empty<string>()
            }
        });
    }
}

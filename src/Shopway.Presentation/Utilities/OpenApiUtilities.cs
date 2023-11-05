using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;

namespace Shopway.Presentation.Utilities;

public static class OpenApiUtilities
{
    private const string JwtAuthorizationHeader = "Authorization";
    private const string ApiKeyAuthorizationHeader = "x-api-key";
    private const string JwtAuthorizationSecurityName = "oauth2";
    private const string Jwt = nameof(Jwt);
    private const string ApiKeyScheme = nameof(ApiKeyScheme);
    private const string Bearer = nameof(Bearer);
    private const string ApiKey = nameof(ApiKey);

    public static void AddJwtAuthorization(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(JwtAuthorizationSecurityName, new OpenApiSecurityScheme
        {
            Description = $"Bearer authorization. Input: \"{{token}}\"",
            In = ParameterLocation.Header,
            Name = JwtAuthorizationHeader,
            Type = SecuritySchemeType.Http,
            BearerFormat = Jwt,
            Scheme = Bearer
        });

        options.ConfigureRequirement(Bearer);
    }

    public static void AddApiKeyAuthorization(this SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(ApiKey, new OpenApiSecurityScheme
        {
            Description = $"ApiKey authorization. Input: \"{{apikey}}\"",
            In = ParameterLocation.Header,
            Name = ApiKeyAuthorizationHeader,
            Type = SecuritySchemeType.ApiKey,
            Scheme = ApiKeyScheme
        });

        options.ConfigureRequirement(ApiKey);
    }

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

    /// <summary>
    /// Provides xml documentation for OpenApi. 
    /// </summary>
    /// <param name="options">OpenApi options</param>
    /// <param name="assembly">Assembly containing controllers</param>
    public static void IncludeXmlDocumentation(this SwaggerGenOptions options, Assembly assembly)
    {
        var xmlFile = $"{assembly.GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        options.IncludeXmlComments(xmlPath);

        //Note: Paste the following xml to the project file that contains controllers
        /*
        <PropertyGroup>
		    <GenerateDocumentationFile>true</GenerateDocumentationFile>
		    <NoWarn>$(NoWarn);1591</NoWarn>
	    </PropertyGroup>
        */
    }

    private static void ConfigureRequirement(this SwaggerGenOptions options, string referenceId)
    {
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = referenceId
                    },
                    In = ParameterLocation.Header
                },
                Array.Empty<string>()
            }
        });
    }
}

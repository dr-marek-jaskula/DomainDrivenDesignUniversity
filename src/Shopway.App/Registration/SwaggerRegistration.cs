using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerRegistration
{
    private const string JwtAuthorizationHeader = "Authorization";
    private const string ApiKeyAuthorization = "x-api-key";
    private const string JwtAuthorizationName = "oauth2";
    private const string Jwt = nameof(Jwt);
    private const string ApiKeyScheme = nameof(ApiKeyScheme);
    private const string Bearer = nameof(Bearer);
    private const string ApiKey = nameof(ApiKey);

    public static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureOpenApiOptions>();

        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<OpenApiDefaultValues>();

            options.ExampleFilters();

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>();

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
        });

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        services.AddSwaggerExamplesFromAssemblies(Shopway.App.AssemblyReference.Assembly);

        return services;
    }

    public static IApplicationBuilder ConfigureSwagger(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.InjectStylesheet("/swaggerstyles/SwaggerDark.css");
            });
        }

        return app;
    }


}

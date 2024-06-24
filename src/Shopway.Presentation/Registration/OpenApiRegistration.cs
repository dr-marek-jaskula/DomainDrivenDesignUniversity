using Asp.Versioning.ApiExplorer;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Shopway.Presentation.OpenApi;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection;

internal static class OpenApiRegistration
{
    private const string SwaggerDefaultDocumentTitle = $"{nameof(Shopway)}";
    private const string SwaggerDarkThameStyleFileName = "SwaggerDark.css";
    private const string WwwRootDirectoryName = "OpenApi";
    private const string ShopwayPresentation = $"{nameof(Shopway)}.{nameof(Shopway.Presentation)}";

    internal static IServiceCollection RegisterOpenApi(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, OpenApiOptionsSetup>();

        services.AddSwaggerGen(options =>
        {
            options.OperationFilter<OpenApiDefaultValues>();
            options.ExampleFilters();
            options.IncludeXmlDocumentation(Shopway.Presentation.AssemblyReference.Assembly);
            options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>();
            options.AddJwtAuthorization();
            options.AddApiKeyAuthorization();
        });

        services.AddSwaggerExamplesFromAssemblies(Shopway.Presentation.AssemblyReference.Assembly);

        services.SetSwaggerForEndpoints();

        return services;
    }

    private static IServiceCollection SetSwaggerForEndpoints(this IServiceCollection services)
    {
        return services
            .SwaggerDocument(options =>
            {
                options.MaxEndpointVersion = 1;
                options.DocumentSettings = documentSettings =>
                {
                    documentSettings.DocumentName = "Release 1.0";
                    documentSettings.Title = SwaggerDefaultDocumentTitle;
                };

                options.AutoTagPathSegmentIndex = 0;
            })
            .SwaggerDocument(options =>
            {
                options.MaxEndpointVersion = 2;
                options.DocumentSettings = documentSettings =>
                {
                    documentSettings.DocumentName = "Release 2.0";
                    documentSettings.Title = SwaggerDefaultDocumentTitle;
                };

                options.AutoTagPathSegmentIndex = 0;
            });
    }

    internal static IApplicationBuilder ConfigureOpenApi(this IApplicationBuilder app, bool isDevelopment)
    {
        if (isDevelopment)
        {
            var provider = app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();

            UseStaticFiles(app);

            app.UseSwaggerUI(options =>
            {
                foreach (var groupName in provider.ApiVersionDescriptions.Select(x => x.GroupName))
                {
                    options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
                }

                options.InjectStylesheet($"/{SwaggerDarkThameStyleFileName}");
            });
        }

        return app;
    }

    /// <summary>
    /// Used for the WebApplicationFactory (tests) reason.
    /// </summary>
    private static void UseStaticFiles(IApplicationBuilder app)
    {
        var wwwRoot = Path.Combine($"{Directory.GetParent(Directory.GetCurrentDirectory())}", ShopwayPresentation, WwwRootDirectoryName);

        try
        {
            app
                .UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(wwwRoot)
                });
        }
        catch
        {
            app
                .UseStaticFiles(new StaticFileOptions()
                {
                    FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
                });
        }
    }
}

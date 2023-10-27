using Shopway.App.Utilities;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Microsoft.Extensions.DependencyInjection;

public static class OpenApiRegistration
{
    public static IServiceCollection RegisterOpenApi(this IServiceCollection services)
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
        
        services.AddSwaggerExamplesFromAssemblies(Shopway.App.AssemblyReference.Assembly);

        return services;
    }

    public static IApplicationBuilder ConfigureOpenApi(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                foreach (var groupName in provider.ApiVersionDescriptions.Select(x => x.GroupName))
                {
                    options.SwaggerEndpoint($"/swagger/{groupName}/swagger.json", groupName.ToUpperInvariant());
                }

                options.InjectStylesheet("/swaggerstyles/SwaggerDark.css");
            });
        }

        return app;
    }
}

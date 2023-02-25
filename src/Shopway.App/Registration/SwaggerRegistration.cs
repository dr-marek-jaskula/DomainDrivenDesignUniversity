using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Swashbuckle.AspNetCore.Filters;
using Shopway.App.Utilities;

namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerRegistration
{
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

            options.AddJwtAuthrization();
            options.AddApiKeyAuthrization();
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

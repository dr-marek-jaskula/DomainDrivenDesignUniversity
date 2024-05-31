using Asp.Versioning;
using Asp.Versioning.Conventions;
using FastEndpoints;
using FastEndpoints.AspVersioning;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Shopway.Presentation.Resolvers;

namespace Microsoft.Extensions.DependencyInjection;

internal static class FastEndpointsRegistration
{
    private static JsonSerializerSettings _jsonSerializerSettings = new()
    {
        ContractResolver = new RequiredPropertiesCamelCaseContractResolver(),
        Formatting = Formatting.Indented,
        Converters = [new ValidationProblemDetailsConverter(), new ProblemDetailsConverter(), new StringEnumConverter()],
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    internal static IServiceCollection RegisterEndpoints(this IServiceCollection services)
    {
        services
           .AddFastEndpoints();

        return services;
    }

    internal static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
    {
        app
            .UseFastEndpoints(options =>
            {
                options.Endpoints.RoutePrefix = "api/endpoints";

                options.Endpoints.Configurator = configure =>
                {
                    configure.Description(builder => builder.ProducesProblem(StatusCodes.Status400BadRequest));
                };
                options.Serializer.RequestDeserializer = async (request, dto, jCtx, cancellationToken) =>
                {
                    using var reader = new StreamReader(request.Body);
                    return JsonConvert.DeserializeObject(await reader.ReadToEndAsync(), dto, _jsonSerializerSettings);
                };
            })
            .UseSwaggerGen();

        return app;
    }
}

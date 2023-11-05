using Newtonsoft.Json.Converters;
using Shopway.Presentation.Resolvers;
using static Newtonsoft.Json.ReferenceLoopHandling;
using static Newtonsoft.Json.Formatting;
using ApiBehaviorOptions = Shopway.Presentation.Options.ApiBehaviorOptions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ControllerRegistration
{
    internal static IServiceCollection RegisterControllers(this IServiceCollection services)
    {
        services
            .AddControllers()
            .AddApplicationPart(Shopway.Presentation.AssemblyReference.Assembly)
            .ConfigureApiBehaviorOptions(options =>
                options.InvalidModelStateResponseFactory = ApiBehaviorOptions.InvalidModelStateResponse)
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new RequiredPropertiesCamelCaseContractResolver();
                options.SerializerSettings.Formatting = Indented;
                options.SerializerSettings.Converters.Add(new StringEnumConverter());
                options.SerializerSettings.ReferenceLoopHandling = Ignore;
            });

        return services;
    }
}

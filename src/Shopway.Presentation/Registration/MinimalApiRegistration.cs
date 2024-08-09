using Microsoft.AspNetCore.Builder;
using Shopway.Presentation;
using Shopway.Presentation.Abstractions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalApiRegistration
{
    internal static IApplicationBuilder UseMinimalApiEndpoints(this WebApplication app)
    {
        var endpointTypes = GetEndpointTypes();
        var baseGroup = BaseEndpointGroup.RegisterEndpointGroup(app);

        foreach (var endpointType in endpointTypes)
        {
            var endpointGroup = GetEndpointGroupType(endpointType);
            var endpointRegisterMethod = endpointType.GetStaticMethod(nameof(IEndpoint.RegisterEndpoint));

            if (endpointGroup is null)
            {
                endpointRegisterMethod.Invoke(null, [baseGroup]);
                continue;
            }

            var endpointGroupRegisterMethod = endpointGroup.GetStaticMethod(nameof(IEndpointGroup.RegisterEndpointGroup));
            var routeGroupBuilder = endpointGroupRegisterMethod.Invoke(null, [baseGroup]);
            endpointRegisterMethod.Invoke(null, [routeGroupBuilder]);
        }

        return app;
    }

    private static TypeInfo[] GetEndpointTypes()
    {
        return AssemblyReference.Assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .ToArray();
    }

    private static Type? GetEndpointGroupType(TypeInfo endpoint)
    {
        return endpoint
            .GetInterfaces()
            .First(x => x.IsGenericType && x.Name.Contains(nameof(IEndpoint)))
            .GetGenericArguments()
            .FirstOrDefault(x => x.IsAssignableTo(typeof(IEndpointGroup)));
    }

    private static MethodInfo GetStaticMethod(this Type type, string name)
    {
        return type.GetMethod(name, BindingFlags.Static | BindingFlags.Public)!;
    }
}

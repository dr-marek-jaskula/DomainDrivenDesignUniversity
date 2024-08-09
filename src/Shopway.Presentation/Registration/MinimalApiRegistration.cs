using Microsoft.AspNetCore.Builder;
using Shopway.Presentation;
using Shopway.Presentation.Abstractions;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class MinimalApiRegistration
{
    internal static IApplicationBuilder UseMinimalApiEndpoints(this WebApplication app)
    {
        var baseGroup = BaseEndpointGroup.RegisterEndpointGroup(app);

        var endpointInfos = AssemblyReference.Assembly
            .GetEndpointTypes()
            .Select(type => (type.GetEndpointRegisterMethod(), type.GetEndpointGroupType()));

        foreach ((MethodInfo endpointRegisterMethod, Type? endpointGroupType) in endpointInfos)
        {
            var endpointGroupRegisterMethod = endpointGroupType.GetEndpointGroupRegisterMethodOrDefault();
            endpointRegisterMethod.RegisterEndpoint(baseGroup, endpointGroupRegisterMethod);
        }

        return app;
    }

    private static TypeInfo[] GetEndpointTypes(this Assembly assembly)
    {
        return assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } && type.IsAssignableTo(typeof(IEndpoint)))
            .ToArray();
    }

    private static Type? GetEndpointGroupType(this TypeInfo endpoint)
    {
        return endpoint
            .GetInterfaces()
            .Single(x => x.IsGenericType && x.Name.Contains(nameof(IEndpoint)))
            .GetGenericArguments()
            .SingleOrDefault(x => x.IsAssignableTo(typeof(IEndpointGroup)));
    }

    private static void RegisterEndpoint(this MethodInfo registerEndpointMethod, object group, MethodInfo? registerEndpointGroupMethod = null)
    {
        if (registerEndpointGroupMethod is null)
        {
            registerEndpointMethod.Invoke(null, [group]);
        }

        var routeGroupBuilder = registerEndpointGroupMethod!.Invoke(null, [group]);
        registerEndpointMethod.Invoke(null, [routeGroupBuilder]);
    }

    private static MethodInfo? GetEndpointGroupRegisterMethodOrDefault(this Type? endpointGroupType)
    {
        if (endpointGroupType is null)
        {
            return null;
        }

        return endpointGroupType.GetStaticMethod(nameof(IEndpointGroup.RegisterEndpointGroup));
    }

    private static MethodInfo GetEndpointRegisterMethod(this TypeInfo endpointType)
    {
        return endpointType.GetStaticMethod(nameof(IEndpoint.RegisterEndpoint));
    }

    private static MethodInfo GetStaticMethod(this Type type, string name)
    {
        return type.GetMethod(name, BindingFlags.Static | BindingFlags.Public)!;
    }
}

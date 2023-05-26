using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Utilities;

public static class TypeUtilities
{
    public static bool IsResult(this Type type)
    {
        return type.GetInterfaces().Any(interfaceType => interfaceType == typeof(IResult));
    }

    public static bool IsGenericResult(this Type type)
    {
        return type.IsGenericType && type.GetInterfaces().Any(interfaceType => interfaceType == typeof(IResult));
    }

    public static Type? GetUnderlyingType(this Type type)
    {
        return type.IsGenericResult() 
            ? type.GetGenericArguments()[0]
            : null;
    }
}

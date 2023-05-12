using Shopway.Domain.Abstractions;
using System.Reflection;

namespace Shopway.Domain.Utilities;

public static class ReflectionUtilities
{
    public static Type GetEntityTypeFromEntityId(this PropertyInfo entityId)
    {
        if (entityId.PropertyType is not IEntityId entityType)
        {
            throw new ArgumentException("Provided property info does not implement IEntityId interface");
        }

        return entityType.GetEntityTypeFromEntityId();
    }

    public static Type GetEntityTypeFromEntityId(this IEntityId entityId)
    {
        var assembly = Shopway.Domain.AssemblyReference.Assembly;

        var skipAmount = IEntityId.Id.Length;

        var typeName = entityId.GetType().Name[0..^skipAmount];

        return assembly
            .GetTypes()
            .Where(type => type.Name == typeName)
            .Single();
    }

    public static MethodInfo GetFirstGenericMethod(this Type baseType, string methodName, params Type[] genericType)
    {
        var methodFormBaseType = baseType
            .GetMethods()
            .Where(method => method.Name == methodName)
            .First()!;

        return methodFormBaseType.MakeGenericMethod(genericType);
    }
}
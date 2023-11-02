using System.Reflection;
using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Utilities;

public static class ReflectionUtilities
{
    public static Type GetEntityTypeFromEntityIdType(Type entityIdType)
    {
        if (entityIdType.Implements<IEntityId>() is false)
        {
            throw new ArgumentException($"{entityIdType.Name} must implements {nameof(IEntityId)}");
        }

        var skipAmount = IEntityId.Id.Length;
        var typeName = entityIdType.Name[0..^skipAmount];

        return Domain.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.Implements<IEntity>())
            .Where(type => type.Name == typeName)
            .Single();
    }

    public static Type[] GetEntityIdTypes()
    {
        return Domain.AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.Implements<IEntityId>())
            .Where(type => type.IsValueType)
            .ToArray();
    }

    public static Type GetEntityTypeFromEntityId(this IEntityId entityId)
    {
        return GetEntityTypeFromEntityIdType(entityId.GetType());
    }

    public static MethodInfo GetSingleGenericMethod(this Type baseType, string methodName, params Type[] genericType)
    {
        var methodFormBaseType = baseType
            .GetMethod(methodName);

        if (methodFormBaseType is null || methodFormBaseType.IsGenericMethod is false)
        {
            throw new ArgumentException($"{baseType.Name} does not contain generic method {methodName}");
        }

        return methodFormBaseType.MakeGenericMethod(genericType);
    }

    public static IEntityId GetEntityIdFromEntity(this IEntity baseType)
    {
        return (IEntityId)baseType
                .GetType()
                .GetProperty(IEntityId.Id)!
                .GetValue(baseType)!;
    }

    public static bool Implements<TInterface>(this PropertyInfo property)
    {
        return property
            .PropertyType
            .Implements<TInterface>();
    }

    public static bool Implements<TInterface>(this Type baseType)
    {
        return baseType
            .GetInterface(typeof(TInterface).Name) is not null;
    }

    public static Type[] GetTypesWithAnyMatchingInterface(this Assembly assembly, Func<Type, bool> typeInterfaceMatch)
    {
        return assembly
            .GetTypes()
            .Where(type => type.GetInterfaces().Any(typeInterfaceMatch))
            .ToArray();
    }
}
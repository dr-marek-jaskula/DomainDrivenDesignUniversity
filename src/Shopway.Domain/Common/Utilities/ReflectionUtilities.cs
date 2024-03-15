using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using System.Reflection;

namespace Shopway.Domain.Common.Utilities;

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

        return AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.Implements<IEntity>())
            .Where(type => type.Name == typeName)
            .Single();
    }

    public static Type[] GetEntityIdTypes()
    {
        return AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.Implements<IEntityId>())
            .Where(type => type.IsValueType)
            .ToArray();
    }

    public static Type GetEntityTypeFromEntityId(this IEntityId entityId)
    {
        return GetEntityTypeFromEntityIdType(entityId.GetType());
    }

    public static bool IsValueObject(this Type type)
    {
        return type.BaseType == typeof(ValueObject);
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

    public static IEnumerable<Type> GetTypesWithAnyMatchingInterface(this Assembly assembly, Func<Type, bool> typeInterfaceMatch)
    {
        return assembly
            .GetTypes()
            .Where(type => type.GetInterfaces().Any(typeInterfaceMatch));
    }

    public static bool IsGenericEnumerable(this Type type, out Type propertyType)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            propertyType = type.GenericTypeArguments[0];

            return true;
        }

        propertyType = type;

        return false;
    }

    public static bool IsGeneric(this Type type, out Type propertyType)
    {
        if (type.IsGenericType)
        {
            propertyType = type.GenericTypeArguments[0];

            return true;
        }

        propertyType = type;

        return false;
    }

    public static object GetProperty<TType>(this TType type, string propertyName)
    {
        return typeof(TType).GetProperty(propertyName)!.GetValue(type)!;
    }

    //Without caching the delegate the use of these two methods is much slower
    //public static string GetPropertyAsString<TType>(this TType type, string propertyName)
    //{
    //    var getMethod = typeof(TType).GetProperty(propertyName)!.GetGetMethod()!;

    //    Func<TType, object> getter = (Func<TType, object>)Delegate
    //        .CreateDelegate(typeof(Func<TType, object>), null, typeof(TType).GetProperty(propertyName)!.GetGetMethod()!);

    //    return getter(type).ToString()!;
    //}

    //public static TPropertyType GetProperty<TType, TPropertyType>(this TType type, string propertyName)
    //{
    //    Func<TType, TPropertyType> getter = (Func<TType, TPropertyType>)Delegate
    //        .CreateDelegate(typeof(Func<TType, TPropertyType>), null, typeof(TType).GetProperty(propertyName)!.GetGetMethod()!);

    //    return getter(type);
    //}
}
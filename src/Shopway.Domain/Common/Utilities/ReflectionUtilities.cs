using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using System.Collections.Immutable;
using System.Reflection;

namespace Shopway.Domain.Common.Utilities;

public static class ReflectionUtilities
{
    public static bool EntityOrNotValueObjectWithSingleValue(this Type type)
    {
        return type.Implements<IEntity>() || NotValueObjectWithSingleValue(type);
    }

    public static bool NotValueObjectWithSingleValue(this Type type)
    {
        return type.IsAssignableTo(typeof(IValueObject)) && type.GetProperties().Length > 1;
    }

    public static bool EntityOrNotValueObjectWithSingleValue(this PropertyInfo propertyInfo)
    {
        return propertyInfo.Implements<IEntity>() || NotValueObjectWithSingleValue(propertyInfo);
    }

    public static bool NotValueObjectWithSingleValue(this PropertyInfo propertyInfo)
    {
        return propertyInfo.PropertyType.NotValueObjectWithSingleValue();
    }

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

    public static Type? GetEntityGenericKeyTypeFromEntityIdTypeOrDefault(Type entityIdType)
    {
        if (entityIdType.Implements<IEntityId>() is false)
        {
            throw new ArgumentException($"{entityIdType.Name} must implements {nameof(IEntityId)}");
        }

        var typeName = entityIdType.Name.Replace(IEntityId.Id, IUniqueKey.Key);

        return AssemblyReference.Assembly
            .GetTypes()
            .Where(type => type.ImplementsGeneric<IUniqueKey>())
            .Where(type => type.Name == typeName)
            .SingleOrDefault();
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
        return type.IsAssignableTo(typeof(IValueObject));
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

    public static MethodInfo GetSingleGenericMethod(this Type baseType, string methodName, BindingFlags bindingFlags, params Type[] genericType)
    {
        var methodFormBaseType = baseType
            .GetMethod(methodName, bindingFlags);

        if (methodFormBaseType is null || methodFormBaseType.IsGenericMethod is false)
        {
            throw new ArgumentException($"{baseType.Name} does not contain generic method {methodName}");
        }

        return methodFormBaseType.MakeGenericMethod(genericType);
    }

    public static MethodInfo GetStaticMethod(this Type type, string name)
    {
        return type.GetMethod(name, BindingFlags.Static | BindingFlags.Public)!;
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

    public static bool ImplementsGeneric<TInterface>(this Type baseType)
    {
        var @interface = baseType
            .GetInterfaces()
            .FirstOrDefault(i => i.Name.Contains(typeof(TInterface).Name));

        return @interface is not null
            && @interface.IsGenericType;
    }

    public static ImmutableArray<Type> GetTypesThatInjectGeneric(this Assembly assembly, Type genericInjectedType)
    {
        if (genericInjectedType.IsGenericType is false)
        {
            throw new InvalidOperationException($"Type '{genericInjectedType} must be a generic type'");
        }

        return assembly.GetTypes()
            .Where(t => t.IsValueType is false)
            .Where(t => t.GetConstructors().Length is 1)
            .Where(t => t.GetConstructors().Single()
                .GetParameters()
                .Where(x => x.ParameterType.IsGenericType && x.ParameterType.IsValueType is false)
                .Any(x => x.ParameterType.IsAssignableToGenericType(genericInjectedType)))
            .ToImmutableArray();
    }

    public static bool IsAssignableToGenericType(this Type givenType, Type genericType)
    {
        var interfaceTypes = givenType.GetInterfaces();

        foreach (var it in interfaceTypes)
        {
            if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
            {
                return true;
            }
        }

        if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
        {
            return true;
        }

        Type? baseType = givenType.BaseType;

        if (baseType is null)
        {
            return false;
        }

        return IsAssignableToGenericType(baseType, genericType);
    }

    public static IEnumerable<Type> GetTypesWithAnyMatchingInterface(this Assembly assembly, Func<Type, bool> typeInterfaceMatch)
    {
        return assembly
            .GetTypes()
            .Where(type => type.GetInterfaces().Any(typeInterfaceMatch));
    }

    public static IEnumerable<Type> GetKeyTypes()
    {
        return Domain.AssemblyReference.Assembly
            .GetTypesWithAnyMatchingInterface(i => i.Name.Contains(nameof(IUniqueKey)))
            .Where(type => type.IsInterface is false);
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

    public static bool IsEnumerableType(this Type type)
    {
        return type.GetInterface("IEnumerable") is not null;
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

    public static bool HasProperty(this Type type, string propertyName)
    {
        return type.GetProperty(propertyName) is not null;
    }

    public static object? GetProperty<TType>(this TType type, string propertyName)
    {
        return typeof(TType).GetProperty(propertyName)?.GetValue(type);
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

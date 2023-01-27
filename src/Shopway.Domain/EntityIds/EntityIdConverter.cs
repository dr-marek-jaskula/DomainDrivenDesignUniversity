using Shopway.Domain.Abstractions;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Shopway.Domain.EntityIds;

public sealed class EntityIdConverter<TEntitiyId> : TypeConverter
    where TEntitiyId : struct, IEntityId<TEntitiyId>
{
    private readonly Type _type;

    public EntityIdConverter(Type type)
    {
        _type = type;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string)
            || base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
    {
        return destinationType == typeof(string)
            || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (value is string @string)
        {
            var guid = Guid.Parse(@string);
            var createMethod = _type.GetMethod(nameof(IEntityId<TEntitiyId>.Create));
            return createMethod!.Invoke(null, new object[] { guid });
        }

        return base.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var entityId = (IEntityId<TEntitiyId>)value;
        var idValue = entityId.Value;

        if (destinationType == typeof(string))
        {
            return idValue.ToString()!;
        }

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

public sealed class EntityIdConverter : TypeConverter
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> ActualConverters = new();

    private readonly TypeConverter _innerConverter;

    public EntityIdConverter(Type entityIdType)
    {
        _innerConverter = ActualConverters.GetOrAdd(entityIdType, CreateInnerConverter);
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return _innerConverter.CanConvertFrom(sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType)
    {
        return _innerConverter.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        return _innerConverter.ConvertFrom(context, culture, value);
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        return _innerConverter.ConvertTo(context, culture, value, destinationType);
    }

    private static TypeConverter CreateInnerConverter(Type entityIdType)
    {
        if (IsEntityId(entityIdType, out var idType) is false)
        {
            throw new InvalidOperationException($"'{entityIdType}' is not an entity id");
        }

        var actualConverterType = typeof(EntityIdConverter<>).MakeGenericType(idType);
        return (TypeConverter)Activator.CreateInstance(actualConverterType, entityIdType)!;
    }

    public static bool IsEntityId(Type type, [NotNullWhen(true)] out Type? idType)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }
        
        var @interface = type
            .GetInterfaces()
            .Where(@interface => @interface.Name.StartsWith(nameof(IEntityId)))
            .Where(@interface => @interface.GetGenericArguments()[0] == type)
            .FirstOrDefault();

        if (@interface is not null)
        {
            idType = @interface.GetGenericArguments()[0];
            return true;
        }

        idType = null;
        return false;
    }
}
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Errors;
using Shopway.Domain.Common.Results;
using Shopway.Domain.Common.Utilities;
using System.Collections.Concurrent;
using System.Reflection;
using static Shopway.Domain.Common.Utilities.ListUtilities;

namespace Shopway.Domain.Common.BaseTypes;

public static class IEntityUtilities
{
    private static readonly ConcurrentDictionary<string, List<string>> _entityPropertiesCache = new();

    public static List<string> GetEntityPropertiesWithHierarchy(string entityName)
    {
        return _entityPropertiesCache.GetOrAdd(entityName, key =>
        {
            var entitiyType = AssemblyReference.Assembly
                .GetTypes()
                .Where(x => x.Name == entityName)
                .First(x => x.Implements<IEntity>());

            var allProperties = GetEntityPropertiesWithHierarchy(entitiyType, string.Empty);

            return allProperties
                .Select(x => x.NameWithHierarchy)
                .ToList();
        });
    }

    public static Result ValidateEntityProperties(string entity, List<string> requestedProperties)
    {
        var entityProperties = GetEntityPropertiesWithHierarchy(entity);

        var invalidRequestedProperties = requestedProperties.Except(entityProperties);

        if (invalidRequestedProperties.Any())
        {
            var errors = EmptyList<Error>();

            foreach (var property in invalidRequestedProperties)
            {
                errors.Add(Error.InvalidArgument(property));
            }

            return ValidationResult.WithErrors(errors);
        }

        return Result.Success();
    }

    private static List<(PropertyInfo Info, string NameWithHierarchy)> GetEntityPropertiesWithHierarchy(Type entitiyType, string previousName)
    {
        List<(PropertyInfo Info, string NameWithHierarchy)> entityProperties = entitiyType
            .GetProperties()
            .Where(x => x.Name.NotContains(nameof(DomainEvent)))
            .Select(x => (x, previousName.NotNullOrEmptyOrWhiteSpace() ? $"{previousName}.{x.Name}" : x.Name))
            .ToList();

        var nestedEntitiesOrNotValueObjectsWithSingleValue = entityProperties
            .Where(x =>
            {
                if (x.Info.EntityOrNotValueObjectWithSingleValue())
                {
                    return true;
                }

                Type? enumerablePropertyType = null;
                var isPublicGenericEnumerable = x.Info.PropertyType.IsPublic && x.Info.PropertyType.IsGeneric(out enumerablePropertyType);

                if (isPublicGenericEnumerable)
                {
                    return enumerablePropertyType!.EntityOrNotValueObjectWithSingleValue();
                }

                return false;
            })
            .ToList();

        foreach (var (info, nameWithHierarchy) in nestedEntitiesOrNotValueObjectsWithSingleValue)
        {
            var nestedProperties = info.PropertyType.IsGeneric(out var genericType)
                ? GetEntityPropertiesWithHierarchy(genericType, nameWithHierarchy)
                : GetEntityPropertiesWithHierarchy(info.PropertyType, nameWithHierarchy);

            entityProperties.AddRange(nestedProperties);
        }

        return entityProperties;
    }
}

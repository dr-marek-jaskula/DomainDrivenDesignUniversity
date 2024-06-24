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
    private static readonly ConcurrentDictionary<string, List<string>> _aggregatePropertiesCache = new();

    public static bool IsEntity(string entityName)
    {
        return AssemblyReference.Assembly
            .GetTypes()
            .Where(x => x.Name == entityName)
            .Any(x => x.Implements<IEntity>());
    }

    public static bool IsAggregateRoot(string entityName)
    {
        return AssemblyReference.Assembly
            .GetTypes()
            .Where(x => x.Name == entityName)
            .Any(x => x.Implements<IAggregateRoot>());
    }

    public static List<string> GetAggregatePropertiesWithHierarchy(string entityName)
    {
        return _aggregatePropertiesCache.GetOrAdd(entityName, key =>
        {
            var entitiyType = AssemblyReference.Assembly
                .GetTypes()
                .Where(x => x.Name == entityName)
                .First(x => x.Implements<IAggregateRoot>());

            var allProperties = GetEntityPropertiesWithHierarchy(entitiyType, string.Empty, []);

            return allProperties
                .Select(x => x.NameWithHierarchy)
                .ToList();
        });
    }

    public static Result ValidateEntityProperties(string entity, List<string> requestedProperties)
    {
        var entityProperties = GetAggregatePropertiesWithHierarchy(entity);

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

    private static List<(PropertyInfo Info, string NameWithHierarchy)> GetEntityPropertiesWithHierarchy(Type entitiyType, string currentName, List<(PropertyInfo info, string nameWithHierarchy)> skipPropertiesToAvoidCyclicReference)
    {
        List<(PropertyInfo Info, string NameWithHierarchy)> entityProperties = entitiyType
            .GetProperties()
            .Where(x => x.Name.NotContains(nameof(DomainEvent)))
            .Select(x => (x, currentName.NotNullOrEmptyOrWhiteSpace() ? $"{currentName}.{x.Name}" : x.Name))
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


        foreach ((PropertyInfo info, string nameWithHierarchy) in nestedEntitiesOrNotValueObjectsWithSingleValue)
        {
            if (skipPropertiesToAvoidCyclicReference.Any(toSkip => toSkip.info == info && nameWithHierarchy.Contains(toSkip.nameWithHierarchy)))
            {
                entityProperties.Remove((info, nameWithHierarchy));
                continue;
            }

            skipPropertiesToAvoidCyclicReference.Add((info, nameWithHierarchy));

            var nestedProperties = info.PropertyType.IsGeneric(out var genericType)
                ? GetEntityPropertiesWithHierarchy(genericType, nameWithHierarchy, skipPropertiesToAvoidCyclicReference)
                : GetEntityPropertiesWithHierarchy(info.PropertyType, nameWithHierarchy, skipPropertiesToAvoidCyclicReference);

            entityProperties.AddRange(nestedProperties);
            entityProperties.Remove((info, nameWithHierarchy));
        }

        return entityProperties;
    }
}

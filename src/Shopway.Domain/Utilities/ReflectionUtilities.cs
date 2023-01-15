using System.Reflection;

namespace Shopway.Domain.Utilities;

public static class ReflectionUtilities
{
    public static Type GetEntityTypeFromEntityId(this PropertyInfo entityId)
    {
        var assembly = Shopway.Domain.AssemblyReference.Assembly;

        var skipAmount = "Id".Length;

        var typeName = entityId.PropertyType.Name[0..^skipAmount];

        return assembly
            .GetTypes()
            .Where(type => type.Name == typeName)
            .Single();
    }
}
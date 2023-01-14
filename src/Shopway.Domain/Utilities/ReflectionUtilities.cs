using System.Reflection;

namespace Shopway.Domain.Utilities;

public static class ReflectionUtilities
{
    public static Type GetEntityTypeFromEntityId(this PropertyInfo entityId)
    {
        var assembly = Shopway.Domain.AssemblyReference.Assembly;

        var skipAmount = "Id".Length;

        var typeName = $"{assembly.GetName().Name}.Entities.{entityId.PropertyType.Name[0..^skipAmount]}";

        return assembly.GetType(typeName)!;
    }
}
using Shopway.Domain.Abstractions;

namespace Shopway.Domain.Errors;

public static class HttpErrors
{
    /// <summary>
    /// Create an Error describing that a password or an email are invalid
    /// </summary>
    public static readonly Error InvalidPasswordOrEmail = new("User.InvalidPasswordOrEmail", "Invalid password or email");

    /// <summary>
    /// Create an Error based on the entity type name and the id that was not found
    /// </summary>
    /// <param name="name">name of the entity type. Use "nameof(TValue)" syntax</param>
    /// <param name="id">id of the entity that was not found</param>
    /// <returns>NotFound error</returns>
    public static Error NotFound<TEntityId>(string name, IEntityId<TEntityId> id)
    {
        return new Error($"{name}.NotFound", $"{name} with Id: '{id.Value}' was not found");
    }

    /// <summary>
    /// Create an Error describing that the provided reference is invalid
    /// </summary>
    /// <returns>InvalidReference error</returns>
    public static Error InvalidReference(Guid reference, string entity)
    {
        return new Error($"Error.{nameof(InvalidReference)}", $"Invalid Entity reference {reference} for entity {entity}");
    }
}

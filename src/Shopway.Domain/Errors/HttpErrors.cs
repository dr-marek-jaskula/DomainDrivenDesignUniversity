namespace Shopway.Domain.Errors;

public static class HttpErrors
{
    /// <summary>
    /// Create an Error based on the entity type name and the id that was not found
    /// </summary>
    /// <param name="name">name of the entity type. Use "nameof(TValue)" syntax</param>
    /// <param name="id">id of the entity that was not found</param>
    /// <returns>NotFound error</returns>
    public static Error NotFound(string name, Guid id)
    {
        return new Error($"{name}.NotFound", $"{name} with Id: '{id}' was not found");
    }
}

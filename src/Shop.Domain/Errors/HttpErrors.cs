namespace Shopway.Domain.Errors;

public static class HttpErrors
{
    public static Error NotFound(string name, Guid id)
    {
        return new Error($"{name}.NotFound", $"{name} with Id: '{id}' was not found");
    }
}

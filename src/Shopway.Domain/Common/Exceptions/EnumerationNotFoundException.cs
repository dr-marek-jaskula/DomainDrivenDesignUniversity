namespace Shopway.Domain.Common.Exceptions;

public sealed class EnumerationNotFoundException : ArgumentOutOfRangeException
{
    public EnumerationNotFoundException(string name, int id)
        : base($"The {name} with the id {id} was not found.")
    {
    }
}

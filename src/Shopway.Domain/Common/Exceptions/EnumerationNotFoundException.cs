namespace Shopway.Domain.Common.Exceptions;

public sealed class EnumerationNotFoundException(string name, int id)
    : ArgumentOutOfRangeException($"The {name} with the id {id} was not found.");

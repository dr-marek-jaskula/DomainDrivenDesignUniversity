namespace Shopway.Domain.Common.Exceptions;

public abstract class DomainException(string message) 
    : Exception(message);
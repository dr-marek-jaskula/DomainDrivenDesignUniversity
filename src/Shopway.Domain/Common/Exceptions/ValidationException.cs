namespace Shopway.Domain.Common.Exceptions;

public sealed class ValidationException(string message) 
    : Exception(message);
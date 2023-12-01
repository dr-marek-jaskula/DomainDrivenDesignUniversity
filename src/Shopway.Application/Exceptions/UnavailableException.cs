namespace Shopway.Application.Exceptions;

public sealed class UnavailableException(string message) : Exception(message);
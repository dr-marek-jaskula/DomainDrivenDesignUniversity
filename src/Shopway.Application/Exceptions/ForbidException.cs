namespace Shopway.Application.Exceptions;

public sealed class ForbidException(string message) : Exception(message);

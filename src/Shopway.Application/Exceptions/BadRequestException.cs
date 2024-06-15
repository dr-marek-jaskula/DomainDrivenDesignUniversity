namespace Shopway.Application.Exceptions;

public sealed class BadRequestException(string message) : Exception(message);

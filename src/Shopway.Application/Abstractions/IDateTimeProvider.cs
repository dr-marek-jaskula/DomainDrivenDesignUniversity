namespace Shopway.Application.Abstractions;

//This will be hard to use in interceptors
public interface IDateTimeProvider
{
    DateTimeOffset UtcNow { get; }
}
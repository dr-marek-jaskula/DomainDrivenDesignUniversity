namespace Shopway.Infrastructure.Providers;

//This will be hard to use in interceptors
public interface IDateTimeProvider
{
    public DateTimeOffset UtcNow { get; }
}
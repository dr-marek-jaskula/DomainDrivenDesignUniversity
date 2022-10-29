using Shopway.Application.Abstractions;

namespace Shopway.Infrastructure.Providers;

public sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
using Microsoft.Extensions.Logging;

namespace Shopway.Infrastructure.Adapters;

public interface ILoggerAdapter<TType>
{
    void LogInformation(string message, params object[] args);

    void Log(LogLevel logLevel, string message, params object[] args);
}
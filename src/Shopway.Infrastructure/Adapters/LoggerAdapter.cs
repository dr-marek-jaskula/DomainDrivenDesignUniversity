using Microsoft.Extensions.Logging;
using Shopway.Application.Abstractions;

namespace Shopway.Infrastructure.Adapters;

public sealed class LoggerAdapter<TType> : ILoggerAdapter<TType>
{
    private readonly ILogger<LoggerAdapter<TType>> _logger;

    public LoggerAdapter(ILogger<LoggerAdapter<TType>> logger)
    {
        _logger = logger;
    }

    public void Log(LogLevel logLevel, string message, params object[] args)
    {
        _logger.Log(logLevel, message, args);
    }

    public void LogInformation(string message, params object[] args)
    {
        Log(LogLevel.Information, message, args);
    }
}
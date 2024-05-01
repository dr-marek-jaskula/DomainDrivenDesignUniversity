using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Quartz;
using Shopway.Infrastructure.Outbox;
using Shopway.Infrastructure.Policies;
using Shopway.Persistence.Framework;

namespace Shopway.Persistence.BackgroundJobs;

//This attribute determines that only one instance of a job will run at once
[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob
(
    ShopwayDbContext dbContext,
    IPublisher publisher,
    ILogger<ProcessOutboxMessagesJob> logger,
    TimeProvider timeProvider,
    IOutboxRepository outboxRepository
)
    : IJob
{
    //We can inject scoped services, because Quartz jobs have scoped lifetime
    private readonly ShopwayDbContext _dbContext = dbContext;
    private readonly IPublisher _publisher = publisher;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger = logger;
    private readonly TimeProvider _timeProvider = timeProvider;
    private readonly IOutboxRepository _outboxRepository = outboxRepository;

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _outboxRepository
            .GetOutboxMessagesAsync(context.CancellationToken);

        if (messages.Length is 0)
        {
            return;
        }

        foreach (var message in messages)
        {
            var domainEvent = message.Deserialize(TypeNameHandling.All);

            if (domainEvent is null)
            {
                _logger.LogDomainEventInvalidDeserialization(message.Content);
                continue;
            }

            var result = await PollyPipelines.AsyncRetryPipeline.ExecuteAndReturnResult(async token =>
                await _publisher.Publish(domainEvent, token), context.CancellationToken);

            message.UpdatePostProcessProperties(_timeProvider.GetUtcNow(), result.Error.MessageOrNullIfErrorNone());
        }

        await _dbContext.SaveChangesAsync();
    }
}

public static partial class LoggerMessageDefinitionsUtilities
{
    [LoggerMessage
    (
        EventId = 6,
        EventName = $"{nameof(ProcessOutboxMessagesJob)}",
        Level = LogLevel.Warning,
        Message = "DomainEvent was not deserialized properly: {Content}",
        SkipEnabledCheck = true
    )]
    public static partial void LogDomainEventInvalidDeserialization(this ILogger logger, string content);
}
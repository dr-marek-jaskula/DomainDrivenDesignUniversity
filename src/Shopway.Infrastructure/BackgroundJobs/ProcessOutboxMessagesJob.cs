using Quartz;
using MediatR;
using Newtonsoft.Json;
using Shopway.Persistence.Outbox;
using Shopway.Persistence.Framework;
using Shopway.Persistence.Utilities;
using Shopway.Infrastructure.Policies;
using Shopway.Application.Abstractions;
using Shopway.Infrastructure.Utilities;

namespace Shopway.Infrastructure.BackgroundJobs;

//This attribute determines that only one instance of a job will run at once
[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    //We can inject scoped services, because Quartz jobs have scoped lifetime
    private readonly ShopwayDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILoggerAdapter<ProcessOutboxMessagesJob> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOutboxRepository _outboxRepository;

    public ProcessOutboxMessagesJob
    (
        ShopwayDbContext dbContext, 
        IPublisher publisher, 
        ILoggerAdapter<ProcessOutboxMessagesJob> logger, 
        IDateTimeProvider dateTimeProvider,
        IOutboxRepository outboxRepository
    )
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
        _outboxRepository = outboxRepository;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _outboxRepository
            .GetOutboxMessagesAsync(context.CancellationToken);

        if (messages.Any() is false)
        {
            return;
        }

        foreach (var message in messages)
        {
            var domainEvent = message.Deserialize(TypeNameHandling.All);

            if (domainEvent is null)
            {
                _logger.LogWarning("DomainEvent was not deserialized properly: {message.Content}", message.Content);
                continue;
            }

            var result = await PollyPipelines.MigrationRetryPipeline.ExecuteAndReturnResult(async token => 
                await _publisher.Publish(domainEvent, token), context.CancellationToken);

            message.UpdatePostProcessProperties(_dateTimeProvider.UtcNow, result.Error.MessageOrNullIfErrorNone());
        }

        await _dbContext.SaveChangesAsync();
    }
}
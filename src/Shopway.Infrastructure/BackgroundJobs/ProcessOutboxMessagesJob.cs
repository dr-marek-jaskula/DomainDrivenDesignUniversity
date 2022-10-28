using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using Quartz;
using Shopway.Domain.DomainEvents;
using Shopway.Infrastructure.Adapters;
using Shopway.Infrastructure.Polly;
using Shopway.Infrastructure.Providers;
using Shopway.Persistence;
using Shopway.Persistence.Outbox;

namespace Shopway.Infrastructure.BackgroundJobs;

//This attribute determines that only one instance of a job will run at once
[DisallowConcurrentExecution]
public sealed class ProcessOutboxMessagesJob : IJob
{
    //We can inject scoped services, because Quartz jobs have scoped lifetime
    private readonly ApplicationDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILoggerAdapter<ProcessOutboxMessagesJob> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublisher publisher, ILoggerAdapter<ProcessOutboxMessagesJob> logger, IDateTimeProvider dateTimeProvider)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOn == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (var message in messages)
        {
            var domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(message.Content);

            if (domainEvent is null)
            {
                _logger.Log(
                    LogLevel.Warning, 
                    "Following DomainEvent was not deserialized properly: {message.Content}", 
                    message.Content);

                continue;
            }

            PolicyResult result = await PollyPolicies.AsyncRetryPolicy.ExecuteAndCaptureAsync(() =>
                _publisher.Publish(domainEvent, context.CancellationToken));

            message.Error = result.FinalException?.ToString();
            message.ProcessedOn = _dateTimeProvider.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
    }
}


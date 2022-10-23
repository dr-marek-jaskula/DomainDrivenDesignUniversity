using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopway.Persistence.Interceptors;
using Shopway.Persistence;
using Shopway.App.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .Scan(
        selector => selector
            .FromAssemblies(
                Shopway.Infrastructure.AssemblyReference.Assembly,
                Shopway.Persistence.AssemblyReference.Assembly)
            .AddClasses(false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

builder.Services.AddMediatR(Shopway.Application.AssemblyReference.Assembly);

//TODO validation pipeline and the other one for domain events
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

builder.Services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

builder.Services.AddValidatorsFromAssembly(
    Shopway.Application.AssemblyReference.Assembly,
    includeInternalTypes: true);

builder.Services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

builder.Services.AddSingleton<UpdateAuditableEntitiesInterceptor>();

//TODO to registration and other object
string connectionString = builder.Configuration.GetConnectionString("Database");

builder.Services.AddDbContext<ApplicationDbContext>(
    (sp, optionsBuilder) =>
    {
        var outboxInterceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()!;
        var auditableInterceptor = sp.GetService<UpdateAuditableEntitiesInterceptor>()!;

        optionsBuilder.UseSqlServer(connectionString)
            .AddInterceptors(
                outboxInterceptor,
                auditableInterceptor);
    });

//TODO something like quartz????????? (logging here and other stuff) QUARTZ is for backgorund jobs?

builder
    .Services
    .AddControllers()
    .AddApplicationPart(Shopway.Presentation.AssemblyReference.Assembly);

builder.Services.AddSwaggerGen();

//TODO go serilog + seq or just serilog to json
builder.Services.AddLogging();

builder.Services.AddTransient<ErrorHandlingMiddleware>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();

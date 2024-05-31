using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Application.Features;
using Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.Features.Proxy;
using Shopway.Application.Features.Proxy.GenericPageQuery;
using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Application.Pipelines;
using Shopway.Domain.Common.BaseTypes;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Results;
using System.Reflection;
using static Shopway.Domain.Common.Utilities.ReflectionUtilities;

namespace Shopway.Application.Registration;

internal static class MediatorRegistration
{
    internal static IServiceCollection RegisterMediator(this IServiceCollection services)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(AssemblyReference.Assembly);
            configuration.AddOpenBehavior(typeof(FluentValidationPipeline<,>));
            configuration.AddOpenBehavior(typeof(LoggingPipeline<,>));
            configuration.AddOpenBehavior(typeof(QueryCachingPipeline<,>));
            configuration.AddBehavior<CreateOrderHeaderOpenTelemetryPipeline>();
        });

        AddOpenHandlers(services);

        return services;
    }

    private static void AddOpenHandlers(IServiceCollection services)
    {
        var entityIdTypes = GetEntityIdTypes();

        foreach (var entityIdType in entityIdTypes)
        {
            Type entityType = GetEntityTypeFromEntityIdType(entityIdType);

            MethodInfo registerGenericQueryMethod = typeof(MediatorRegistration)
                .GetSingleGenericMethod(nameof(RegisterGenericQuery), BindingFlags.NonPublic | BindingFlags.Static, entityType, entityIdType);

            registerGenericQueryMethod.Invoke(null, [services]);

            MethodInfo registerGenericOffsetPageQueryMethod = typeof(MediatorRegistration)
                .GetSingleGenericMethod(nameof(RegisterGenericOffsetPageQuery), BindingFlags.NonPublic | BindingFlags.Static, entityType, entityIdType);

            registerGenericOffsetPageQueryMethod.Invoke(null, [services]);

            MethodInfo registerGenericCursorPageQueryMethod = typeof(MediatorRegistration)
                .GetSingleGenericMethod(nameof(RegisterGenericCursorPageQuery), BindingFlags.NonPublic | BindingFlags.Static, entityType, entityIdType);

            registerGenericCursorPageQueryMethod.Invoke(null, [services]);
        }
    }

    private static IServiceCollection RegisterGenericQuery<TEntity, TEntityId>(IServiceCollection services)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        return services
            .AddTransient<IRequestHandler<GenericQuery<TEntity, TEntityId>, IResult<DataTransferObjectResponse>>, GenericQueryHandler<TEntity, TEntityId>>();
    }

    private static IServiceCollection RegisterGenericOffsetPageQuery<TEntity, TEntityId>(IServiceCollection services)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        return services
            .AddTransient<IRequestHandler<GenericCursorPageQuery<TEntity, TEntityId>, IResult<CursorPageResponse<DataTransferObjectResponse>>>, GenericCursorPageQueryHandler<TEntity, TEntityId>>();
    }

    private static IServiceCollection RegisterGenericCursorPageQuery<TEntity, TEntityId>(IServiceCollection services)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        return services
            .AddTransient<IRequestHandler<GenericOffsetPageQuery<TEntity, TEntityId>, IResult<OffsetPageResponse<DataTransferObjectResponse>>>, GenericOffsetPageQueryHandler<TEntity, TEntityId>>();
    }
}

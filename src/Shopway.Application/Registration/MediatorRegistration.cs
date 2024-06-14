using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shopway.Application.Features;
using Shopway.Application.Features.Orders.Commands.CreateHeaderOrder;
using Shopway.Application.Features.Proxy;
using Shopway.Application.Features.Proxy.GenericPageQuery;
using Shopway.Application.Features.Proxy.GenericQuery.QueryById;
using Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;
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

        AddOpenHandlersWithValidators(services);

        return services;
    }

    private static void AddOpenHandlersWithValidators(IServiceCollection services)
    {
        var entityIdTypes = GetEntityIdTypes();

        foreach (var entityIdType in entityIdTypes)
        {
            Type entityType = GetEntityTypeFromEntityIdType(entityIdType);

            MethodInfo registerGenericQueryByIdMethod = typeof(MediatorRegistration)
                .GetSingleGenericMethod(nameof(RegisterGenericQueryById), BindingFlags.NonPublic | BindingFlags.Static, entityType, entityIdType);

            registerGenericQueryByIdMethod.Invoke(null, [services]);

            MethodInfo registerGenericOffsetPageQueryMethod = typeof(MediatorRegistration)
                .GetSingleGenericMethod(nameof(RegisterGenericOffsetPageQuery), BindingFlags.NonPublic | BindingFlags.Static, entityType, entityIdType);

            registerGenericOffsetPageQueryMethod.Invoke(null, [services]);

            MethodInfo registerGenericCursorPageQueryMethod = typeof(MediatorRegistration)
                .GetSingleGenericMethod(nameof(RegisterGenericCursorPageQuery), BindingFlags.NonPublic | BindingFlags.Static, entityType, entityIdType);

            registerGenericCursorPageQueryMethod.Invoke(null, [services]);

            var entityKeyType = GetEntityGenericKeyTypeFromEntityIdTypeOrDefault(entityIdType);

            if (entityKeyType is null)
            {
                continue;
            }

            MethodInfo registerGenericQueryByKeyMethod = typeof(MediatorRegistration)
                .GetSingleGenericMethod(nameof(RegisterGenericQueryByKey), BindingFlags.NonPublic | BindingFlags.Static, entityType, entityIdType, entityKeyType);

            registerGenericQueryByKeyMethod.Invoke(null, [services]);
        }
    }

    private static IServiceCollection RegisterGenericQueryById<TEntity, TEntityId>(IServiceCollection services)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        return services
            .AddScoped<IRequestHandler<GenericByIdQuery<TEntity, TEntityId>, IResult<DataTransferObjectResponse>>, GenericByIdQueryHandler<TEntity, TEntityId>>();
    }

    private static IServiceCollection RegisterGenericQueryByKey<TEntity, TEntityId, TEntityKey>(IServiceCollection services)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
        where TEntityKey : IUniqueKey<TEntity, TEntityKey>
    {
        return services
            .AddScoped<IRequestHandler<GenericByKeyQuery<TEntity, TEntityId, TEntityKey>, IResult<DataTransferObjectResponse>>, GenericByKeyQueryHandler<TEntity, TEntityId, TEntityKey>>();
    }

    private static IServiceCollection RegisterGenericOffsetPageQuery<TEntity, TEntityId>(IServiceCollection services)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        return services
            .AddScoped<IValidator<GenericCursorPageQuery<TEntity, TEntityId>>, GenericCursorPageQueryValidator<TEntity, TEntityId>>()
            .AddScoped<IRequestHandler<GenericCursorPageQuery<TEntity, TEntityId>, IResult<CursorPageResponse<DataTransferObjectResponse>>>, GenericCursorPageQueryHandler<TEntity, TEntityId>>();
    }

    private static IServiceCollection RegisterGenericCursorPageQuery<TEntity, TEntityId>(IServiceCollection services)
        where TEntity : Entity<TEntityId>
        where TEntityId : struct, IEntityId<TEntityId>
    {
        return services
            .AddScoped<IValidator<GenericOffsetPageQuery<TEntity, TEntityId>>, GenericOffsetPageQueryValidator<TEntity, TEntityId>>()
            .AddScoped<IRequestHandler<GenericOffsetPageQuery<TEntity, TEntityId>, IResult<OffsetPageResponse<DataTransferObjectResponse>>>, GenericOffsetPageQueryHandler<TEntity, TEntityId>>();
    }
}

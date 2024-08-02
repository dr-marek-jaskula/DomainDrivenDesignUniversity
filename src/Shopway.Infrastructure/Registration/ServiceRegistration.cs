using Microsoft.AspNetCore.Identity;
using Scrutor;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.Common.BaseTypes.Abstractions;
using Shopway.Domain.Common.Utilities;
using Shopway.Domain.Orders;
using Shopway.Domain.Users;
using Shopway.Infrastructure.Builders.Batch;
using Shopway.Infrastructure.Services;
using Shopway.Infrastructure.Validators;
using System.Reflection;
using static Shopway.Application.Features.Orders.Commands.BatchUpsertOrderLine.BatchUpsertOrderLineCommand;
using static Shopway.Domain.Common.Utilities.ReflectionUtilities;

namespace Microsoft.Extensions.DependencyInjection;

internal static class ServiceRegistration
{
    internal static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        //Services

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddScoped<IEmailSender, MimeKitEmailSender>();
        services.AddSingleton<IToptService, ToptService>();

        //Validators

        services.AddScoped<IValidator, Validator>();

        //Builders

        services.RegisterBatchResponsBuilderGenericDelegae();
        services.AddScoped(typeof(IBatchResponseBuilder<,>), typeof(BatchResponseBuilder<,>));

        //Scan for the rest

        services.Scan(selector => selector
            .FromAssemblies(
                Shopway.Infrastructure.AssemblyReference.Assembly)
            .AddClasses(false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsMatchingInterface()
            .WithScopedLifetime());

        return services;
    }

    private static void RegisterBatchResponsBuilderGenericDelegae(this IServiceCollection services)
    {
        var typesThatInjectCreateBatchBuilder = Shopway.Application.AssemblyReference.Assembly
            .GetTypesThatInjectGeneric(typeof(CreateBatchResponseBuilder<,>));

        foreach (var typeThatInjectCreateBatchBuilder in typesThatInjectCreateBatchBuilder)
        {
            var (batchRequestType, keyType) = typeThatInjectCreateBatchBuilder.GetRequiredBatchBuilderGenericTypes();

            MethodInfo registerGenericCreateBatchBuilder = typeof(ServiceRegistration)
                .GetSingleGenericMethod(nameof(RegisterGenericCreateBatchBuilder), BindingFlags.NonPublic | BindingFlags.Static, batchRequestType, keyType);

            registerGenericCreateBatchBuilder.Invoke(null, [services]);
        }
    }

    private static (Type batchUpsertRequestType, Type keyType) GetRequiredBatchBuilderGenericTypes(this Type genericInjectedType)
    {
        var genericParameters = genericInjectedType.GetConstructors()
            .Single()
            .GetParameters()
            .Where(x => x.ParameterType.IsGenericType && x.ParameterType.IsValueType is false)
            .Single(x => x.ParameterType.IsAssignableToGenericType(typeof(CreateBatchResponseBuilder<,>)))
            .ParameterType
            .GetGenericArguments();

        var batchUpsertRequestType = genericParameters.Single(x => x.Implements<IBatchRequest>());
        var keyType = genericParameters.Single(x => x.Implements<IUniqueKey>());
        return (batchUpsertRequestType, keyType);
    }

    private static IServiceCollection RegisterGenericCreateBatchBuilder<TBatchUpsertRequest, TKey>(IServiceCollection services)
        where TBatchUpsertRequest : class, IBatchRequest
        where TKey : struct, IUniqueKey
    {
        return services
            .AddSingleton<CreateBatchResponseBuilder<TBatchUpsertRequest, TKey>>(p => BatchResponseBuilderFactory.CreateBuilder);
    }
}

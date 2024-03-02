﻿using Microsoft.AspNetCore.Identity;
using Scrutor;
using Shopway.Application.Abstractions;
using Shopway.Application.Abstractions.CQRS;
using Shopway.Application.Abstractions.CQRS.Batch;
using Shopway.Domain.Users;
using Shopway.Infrastructure.Builders.Batch;
using Shopway.Infrastructure.Services;
using Shopway.Infrastructure.Services.Proxy;
using Shopway.Infrastructure.Validators;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceRegistration
{
    internal static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        //Services

        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserContextService, UserContextService>();
        services.AddSingleton<IMediatorProxyService, MediatorProxyService>();

        //Validators

        services.AddScoped<IValidator, Validator>();

        //Builders

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
}

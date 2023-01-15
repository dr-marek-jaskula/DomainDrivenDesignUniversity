using MediatR;
using Shopway.Domain.Abstractions;
using Shopway.Domain.Entities;
using Shopway.Domain.Errors;
using Shopway.Domain.Utilities;
using Shopway.Persistence.Framework;
using System.Reflection;
using static Shopway.Domain.Errors.HttpErrors;

namespace Shopway.Application.Pipelines.ValidationPipelines;

public sealed class ReferenceValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, IResult
{
    private readonly ShopwayDbContext _context;

    public ReferenceValidationPipeline(ShopwayDbContext context)
    {
        _context = context;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var referenceProperties = typeof(TRequest)
            .GetProperties()
            .Where(prop => prop.PropertyType.GetInterfaces().Any(x => x == typeof(IEntityId)))
            .ToList();

        Error[] errors = referenceProperties
            .Select(async (reference) => await Validate(reference, request))
            .Select(task => task.Result)
            .Where(error => error != Error.None)
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return errors.CreateValidationResult<TResponse>();
        }

        _context.ChangeTracker.Clear();

        return await next();
    }

    private async Task<Error> Validate(PropertyInfo reference, TRequest request)
    {
        //omit optional reference
        if (reference.GetValue(request) is not IEntityId entityId || entityId is null || entityId.Value == Guid.Empty)
        {
            return Error.None; 
        }

        var entityType = reference.GetEntityTypeFromEntityId();

        var entity = await _context.FindAsync(entityType, entityId);

        if (entity is null)
        {
            return InvalidReference(entityId.Value, entityType.Name);
        }

        return Error.None;
    }
}
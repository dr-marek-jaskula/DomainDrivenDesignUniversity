using MediatR;
using Shopway.Domain.Exceptions;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Persistence.Framework;
using System.Reflection;

namespace Shopway.Application.Pipelines;

public sealed class ReferenceValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
	private readonly ApplicationDbContext _context;

	public ReferenceValidationPipeline(ApplicationDbContext context)
	{
		_context = context;
	}

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var referenceProperties = typeof(TRequest)
            .GetProperties()
            .Where(prop => prop.PropertyType.GetInterfaces().Any(x => x == typeof(IEntityId)))
            .ToList();

        foreach (var reference in referenceProperties)
        {
            await Validate(reference, request);
        }

        return await next();
    }

    private async Task Validate(PropertyInfo reference, TRequest request)
    {
        var entityType = GetEntityType(reference);

        if (reference.GetValue(request) is not IEntityId entityId || entityId.Value == Guid.Empty)
        {
            return; //omit optional reference
        }

        var entity = await _context.FindAsync(entityType, entityId);

        if (entity is null)
        {
            throw new ValidationException($"Invalid Entity reference {entityId.Value} for entity {entityType.Name}");
        }
    }

    private static Type GetEntityType(PropertyInfo property)
    {
        return property.PropertyType.BaseType?.Name == typeof(IEntityId<>).Name
            ? property.PropertyType.BaseType.GetGenericArguments().First()
            : property.PropertyType.GetGenericArguments().First();
    }
}
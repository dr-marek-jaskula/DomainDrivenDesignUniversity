using MediatR;
using Shopway.Domain.Errors;
using Shopway.Domain.Results;
using Shopway.Domain.StronglyTypedIds;
using Shopway.Domain.Utilities;
using Shopway.Persistence.Framework;
using System.Reflection;

namespace Shopway.Application.Pipelines;

public sealed class ReferenceValidationPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, IResult
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

        Error[] errors = referenceProperties
            .Select(async (reference) => await Validate(reference, request))
            .Select(t => t.Result)
            .Where(error => error != Error.None)
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return errors.CreateValidationResult<TResponse>();
        }

        return await next();
    }

    private async Task<Error> Validate(PropertyInfo reference, TRequest request)
    {
        if (reference.GetValue(request) is not IEntityId entityId || entityId is null || entityId.Value == Guid.Empty)
        {
            return Error.None; //omit optional reference
        }

        var entityType = GetEntityType(reference);

        var entity = await _context.FindAsync(entityType, entityId);

        if (entity is null)
        {
            return new("Error.InvalidReference", $"Invalid Entity reference {entityId.Value} for entity {entityType.Name}");
        }

        return Error.None;
    }

    private static Type GetEntityType(PropertyInfo property)
    {
        return property.PropertyType.BaseType?.Name == typeof(IEntityId<>).Name
            ? property.PropertyType.BaseType.GetGenericArguments().First()
            : property.PropertyType.GetGenericArguments().First();
    }
}
using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryById;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class GenericByIdQueryStrategyAttribute(string entity) : StrategyAttribute<GenericByIdQueryDiscriminator>
{
    public string Entity { get; } = entity;

    public override GenericByIdQueryDiscriminator ToDiscriminator()
    {
        return new GenericByIdQueryDiscriminator(Entity);
    }
}

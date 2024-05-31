using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class GenericQueryStrategyAttribute(string entity) : StrategyAttribute<GenericQueryDiscriminator>
{
    public string Entity { get; } = entity;

    public override GenericQueryDiscriminator ToDiscriminator()
    {
        return new GenericQueryDiscriminator(Entity);
    }
}

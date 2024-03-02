using Shopway.Domain.Common.Disciminators;

namespace Shopway.Infrastructure.Services.Proxy;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class QueryStrategyAttribute(string entity) : DiscriminatorAttribute<QueryDiscriminator>
{
    public string Entity { get; } = entity;

    public override QueryDiscriminator ToDiscriminator()
    {
        return new QueryDiscriminator(Entity);
    }
}

using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy.Query;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class QueryStrategyAttribute(string entity) : StrategyAttribute<QueryDiscriminator>
{
    public string Entity { get; } = entity;

    public override QueryDiscriminator ToDiscriminator()
    {
        return new QueryDiscriminator(Entity);
    }
}

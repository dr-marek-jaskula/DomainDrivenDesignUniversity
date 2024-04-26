using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class QueryStrategyAttribute(string entity, Type pageType) : StrategyAttribute<QueryDiscriminator>
{
    private readonly Type _pageType = pageType;
    public string Entity { get; } = entity;

    public override QueryDiscriminator ToDiscriminator()
    {
        return new QueryDiscriminator(Entity, _pageType);
    }
}

using Shopway.Domain.Common.Disciminators;

namespace Shopway.Application.Features.Proxy;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class QueryStrategyAttribute(string entity, Type pageType) : DiscriminatorAttribute<QueryDiscriminator>
{
    private readonly Type _pageType = pageType;
    public string Entity { get; } = entity;

    public override QueryDiscriminator ToDiscriminator()
    {
        return new QueryDiscriminator(Entity, _pageType);
    }
}

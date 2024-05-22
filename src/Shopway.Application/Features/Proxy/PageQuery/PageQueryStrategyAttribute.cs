using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy.PageQuery;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class PageQueryStrategyAttribute(string entity, Type pageType) : StrategyAttribute<PageQueryDiscriminator>
{
    private readonly Type _pageType = pageType;
    public string Entity { get; } = entity;

    public override PageQueryDiscriminator ToDiscriminator()
    {
        return new PageQueryDiscriminator(Entity, _pageType);
    }
}

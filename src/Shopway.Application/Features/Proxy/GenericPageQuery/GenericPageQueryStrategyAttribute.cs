using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class GenericPageQueryStrategyAttribute(string entity, string pageName) : StrategyAttribute
{
    public string Entity { get; } = entity;
    public string PageName { get; } = pageName;

    public override string GetKey()
    {
        return GenericProxyPageQuery.GetCacheKey(PageName, Entity);
    }
}

//Option with discriminators
//public sealed record class GenericPageQueryDiscriminator(string Entity, Type PageType) : Discriminator;

//[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
//public sealed class GenericPageQueryStrategyAttribute(string entity, Type pageType) : StrategyAttribute<GenericPageQueryDiscriminator>
//{
//    private readonly Type _pageType = pageType;
//    public string Entity { get; } = entity;

//    public override GenericPageQueryDiscriminator ToDiscriminator()
//    {
//        return new GenericPageQueryDiscriminator(Entity, _pageType);
//    }
//}

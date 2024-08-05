using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class GenericByKeyQueryStrategyAttribute(string entity) : StrategyAttribute
{
    public string Entity { get; } = entity;

    public override string GetKey()
    {
        return GenericProxyByKeyQuery.GetCacheKey(Entity);
    }
}

//Option with discriminators
//[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
//public sealed class GenericByKeyQueryStrategyAttribute(string entity) : StrategyAttribute<GenericByKeyQueryDiscriminator>
//{
//    public string Entity { get; } = entity;

//    public override GenericByKeyQueryDiscriminator ToDiscriminator()
//    {
//        return new GenericByKeyQueryDiscriminator(Entity);
//    }
//}

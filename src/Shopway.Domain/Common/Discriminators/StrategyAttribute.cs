namespace Shopway.Domain.Common.Discriminators;

public abstract class StrategyAttribute<DiscriminatorType> : Attribute
    where DiscriminatorType : Discriminator
{
    public abstract DiscriminatorType ToDiscriminator();
}

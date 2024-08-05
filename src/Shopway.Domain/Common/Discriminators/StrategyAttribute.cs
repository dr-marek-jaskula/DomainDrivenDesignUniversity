namespace Shopway.Domain.Common.Discriminators;

public abstract class StrategyAttribute : Attribute
{
    public abstract string GetKey();
}

public abstract class StrategyAttribute<DiscriminatorType> : Attribute
    where DiscriminatorType : Discriminator
{
    public abstract DiscriminatorType ToDiscriminator();
}

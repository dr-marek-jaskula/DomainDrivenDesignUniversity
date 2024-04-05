namespace Shopway.Domain.Common.Discriminators;

public abstract class DiscriminatorAttribute<DiscriminatorType> : Attribute
    where DiscriminatorType : Discriminator
{
    public abstract DiscriminatorType ToDiscriminator();
}
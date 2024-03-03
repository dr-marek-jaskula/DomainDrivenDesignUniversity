namespace Shopway.Domain.Common.Disciminators;

public abstract class DiscriminatorAttribute<DiscriminatorType> : Attribute
    where DiscriminatorType : Discriminator
{
    public abstract DiscriminatorType ToDiscriminator();
}
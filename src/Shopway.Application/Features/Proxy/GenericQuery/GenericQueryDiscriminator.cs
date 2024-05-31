using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy;

public sealed record class GenericQueryDiscriminator(string Entity) : Discriminator;

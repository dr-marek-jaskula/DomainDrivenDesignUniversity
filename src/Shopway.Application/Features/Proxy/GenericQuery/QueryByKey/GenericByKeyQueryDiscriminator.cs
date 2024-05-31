using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryByKey;

public sealed record class GenericByKeyQueryDiscriminator(string Entity) : Discriminator;

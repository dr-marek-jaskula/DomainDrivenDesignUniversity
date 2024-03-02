using Shopway.Domain.Common.Disciminators;

namespace Shopway.Application.Features.Proxy;

public sealed record class QueryDiscriminator(string Entity) : Discriminator;

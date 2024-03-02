using Shopway.Domain.Common.Disciminators;

namespace Shopway.Infrastructure.Services.Proxy;

public sealed record class QueryDiscriminator(string Entity) : Discriminator;

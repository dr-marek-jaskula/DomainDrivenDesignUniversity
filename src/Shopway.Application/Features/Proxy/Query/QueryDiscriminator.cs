using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy.Query;

public sealed record class QueryDiscriminator(string Entity) : Discriminator;

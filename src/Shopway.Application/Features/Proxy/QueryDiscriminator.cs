using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy;

public sealed record class QueryDiscriminator(string Entity, Type PageType) : Discriminator;

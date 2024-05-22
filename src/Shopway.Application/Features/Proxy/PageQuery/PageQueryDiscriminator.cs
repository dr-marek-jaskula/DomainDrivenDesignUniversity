using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy.PageQuery;

public sealed record class PageQueryDiscriminator(string Entity, Type PageType) : Discriminator;

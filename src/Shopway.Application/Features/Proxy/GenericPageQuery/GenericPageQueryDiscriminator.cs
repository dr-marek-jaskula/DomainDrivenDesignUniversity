using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy;

public sealed record class GenericPageQueryDiscriminator(string Entity, Type PageType) : Discriminator;

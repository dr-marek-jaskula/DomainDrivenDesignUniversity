using Shopway.Domain.Common.Discriminators;

namespace Shopway.Application.Features.Proxy.GenericQuery.QueryById;

public sealed record class GenericByIdQueryDiscriminator(string Entity) : Discriminator;

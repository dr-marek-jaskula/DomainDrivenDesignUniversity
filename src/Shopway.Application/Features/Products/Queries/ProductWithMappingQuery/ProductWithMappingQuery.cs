using Shopway.Application.Abstractions.CQRS;
using Shopway.Domain.Products;
using Shopway.Domain.Products.DataProcessing.Mapping;

namespace Shopway.Application.Features.Products.Queries.DynamicProductWithMappingQuery;

public sealed record ProductWithMappingQuery(ProductId ProductId) : IQuery<DataTransferObjectResponse>
{
    public ProductDynamicMapping? Mapping { get; init; }
}

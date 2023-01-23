using Shopway.Persistence.Specifications.Products;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Requests.Products;

public sealed record QueryProductRequest
(
    int PageNumber,
    int PageSize,
    string? FilterByProductName,
    string? FilterByRevision,
    decimal? FilterByPrice,
    string? FilterByUomCode,
    string? OrderByProductName,
    string? OrderByRevision,
    string? OrderByPrice,
    string? OrderByUomCode,
    string? ThanByProductName,
    string? ThanByRevision,
    string? ThanByPrice,
    string? ThanByUomCode
) : IRequest;
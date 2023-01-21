using Shopway.Persistence.Specifications.Products;
using Shopway.Presentation.Abstractions;

namespace Shopway.Presentation.Requests.Products;

public sealed record QueryProductRequest
(
    int PageNumber,
    int PageSize,
    string? FilterByProductName,
    string? FilterByRevision,
    int? FilterByPrice,
    string? FilterByUomCode,
    string? OrderByProductName,
    string? OrderByByRevision,
    string? OrderByByPrice,
    string? OrderByByUomCode,
    string? ThanByProductName,
    string? ThanByByRevision,
    string? ThanByByPrice,
    string? ThanByByUomCode

) : IRequest;
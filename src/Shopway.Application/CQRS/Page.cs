using Shopway.Domain.Abstractions;

namespace Shopway.Application.CQRS;

public sealed class Page : IPage
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
}
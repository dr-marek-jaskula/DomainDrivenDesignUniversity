namespace Shopway.Domain.Abstractions.Common;

public interface IPage
{
    int PageSize { get; init; }
    int PageNumber { get; init; }
}
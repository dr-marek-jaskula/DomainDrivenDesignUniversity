namespace Shopway.Domain.Abstractions.Common;

public interface IPage
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}
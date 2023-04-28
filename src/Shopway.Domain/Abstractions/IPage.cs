namespace Shopway.Domain.Abstractions;

public interface IPage
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}